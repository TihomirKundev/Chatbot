using System;
using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.DTOs.WebSocket;
using ChatBot.Services.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Linq;
using ChatBot.Models.Enums;

namespace ChatBot.Services;

[SingletonService]
public class WebSocketService : IWebSocketService
{
    private class WebSocketClientCollection : KeyedCollection<User, WebSocketClient>
    {
        protected override User GetKeyForItem(WebSocketClient item) => item.User;
    }

    private readonly WebSocketClientCollection _clients = new();
    private readonly IConversationService _conversationService;
    private readonly IUserService _userService;

    public WebSocketService(IConversationService conversationService, IUserService userService)
    {
        _conversationService = conversationService;
        _conversationService.MessageSent += OnMessageSent;
        _conversationService.ConversationUpdated += OnConversationUpdated;
        _userService = userService;
    }

    public async Task HandleConnection(User user, WebSocket webSocket)
    {
        if (!_clients.TryGetValue(user, out WebSocketClient? webSocketClient) || webSocketClient is null) {
            webSocketClient = new(user);
            webSocketClient.MessageReceived += OnMessageReceived;
            _clients.Add(webSocketClient);
        }

        int leftConnections = await webSocketClient.Listen(webSocket);

        if (leftConnections == 0)
            _clients.Remove(user);
    }

    private void OnMessageReceived(object? sender, WebSocketRequest e)
    {
        if (sender is WebSocketClient client) {
            switch (e)
            {
            case WebSocketRequestSendMessage message:
                OnRequestSendMessage(client, message);
                break;
            case WebSocketRequestNewConversation message:
                OnRequestNewConversation(client, message);
                break;
            case WebSocketRequestGetMessageHistory message:
                OnRequestGetMessageHistory(client, message);
                break;
            case WebSocketRequestGetConversations message:
                OnRequestGetConversations(client, message);
                break;
            case WebSocketRequestGetUserInfo message:
                OnRequestGetUserInfo(client, message);
                break;
            default:
                Debug.Fail("The " + e.GetType() + " subtype wasn't handled");
                break;
            }
        } else
            Debug.Fail("The event handler was called by someone other than WebSocketClient");
    }

    private void OnRequestGetUserInfo(WebSocketClient client, WebSocketRequestGetUserInfo message)
    {
        // if the user isn't admin, we'd have to check if they're in a conversation with the user they want to get info on
        if (client.User.Role == Role.ADMIN && Guid.TryParse(message.User, out var userID) && _userService.GetById(userID) is { } user)
            client.SendMessageAsync(new WebSocketResponseUser(user));
    }

    private void OnRequestGetConversations(WebSocketClient client, WebSocketRequestGetConversations message)
    {
        foreach (var conversation in client.User.Role == Role.ADMIN ? _conversationService.GetConversations() : _conversationService.GetConversationsByUser(client.User)) {
            client.SendMessageAsync(new WebSocketResponseConversation(conversation));
            
            foreach (var participant in conversation.Participants)
                if (participant is User user)
                    client.SendMessageAsync(new WebSocketResponseUser(user));

            foreach (var chatMessage in conversation.Messages)
                client.SendMessageAsync(new WebSocketResponseChatMessage(conversation, chatMessage));
        }
    }

    private void OnRequestGetMessageHistory(WebSocketClient client, WebSocketRequestGetMessageHistory message)
    {
        if (Guid.TryParse(message.Conversation, out Guid conversationID) && _conversationService.GetConversationById(conversationID) is { } conversation && client.IsPrivyToConversation(conversation))
            foreach (var chatMessage in conversation.Messages)
                client.SendMessageAsync(new WebSocketResponseChatMessage(conversation, chatMessage));
    }

    private void OnRequestSendMessage(WebSocketClient sender, WebSocketRequestSendMessage message) {
        if (!string.IsNullOrWhiteSpace(message.Content) && Guid.TryParse(message.Conversation, out Guid conversationID) && _conversationService.GetConversationById(conversationID) is { } conversation && sender.IsPrivyToConversation(conversation))
        {
            if (!Enum.TryParse<QuickSelector>(message.QuickSelector, true, out var quickSelector))
                quickSelector = QuickSelector.CustomerSupport;

            _conversationService.AddMessageToConversation(new MessageDTO {
                AuthorID = sender.User.ID,
                Content = message.Content,
                Nickname = sender.User.FirstName,
                Timestamp = DateTime.Now.ToUnixTimestamp(),
                Action = MessageAction.SEND,
                QuickSelector = quickSelector
            }, conversation.ID);
        }
    }

    private void OnRequestNewConversation(WebSocketClient sender, WebSocketRequestNewConversation message) {
        var conversation = _conversationService.CreateNewConversation();
        _conversationService.AddParticipantToConversation(sender.User.ID, conversation.ID);
        sender.SendMessageAsync(new WebSocketResponseConversation(conversation));
    }

    private void OnMessageSent(Guid conversationID, MessageDTO message)
    {
        if (_conversationService.GetConversationById(conversationID) is { } conversation)
        {
            var response = new WebSocketResponseChatMessage(conversation, message);
            foreach (var client in _clients)
                if (client.IsPrivyToConversation(conversation))
                    client.SendMessageAsync(response);
        }
    }

    private void OnConversationUpdated(Conversation conversation)
    {
        var response = new WebSocketResponseConversation(conversation);
        foreach (var client in _clients)
            if (client.IsPrivyToConversation(conversation)) {
                client.SendMessageAsync(response);
                foreach (var participant in conversation.Participants)
                    if (participant is User user)
                        client.SendMessageAsync(new WebSocketResponseUser(user));
            }
    }
}
