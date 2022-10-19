﻿using ChatBot.Models;
using ChatBot.Models.DTOs;
using ChatBot.Models.Enums;
using ChatBot.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatBot.Services;
using static ChatBot.Services.MessageService;

namespace ChatBot.Middlewares
{
    public class ConversationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IConversationService _conversationService;
        private readonly IParticipantService _participantService;
        private readonly IUserService _userService;
        private readonly IAiClientService _aiClientService;

        public ConversationMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory,
            IConversationService conversationService,
            IParticipantService participantService,
            IUserService userService,
            IAiClientService aiClientService)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ConversationMiddleware>();
            _conversationService = conversationService;
            _participantService = participantService;
            _userService = userService;
            _aiClientService = aiClientService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path != "/ws")
            {
                await _next(context);
                return;
            }
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 404;
                return;
            }
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();

            // !! Everyone so far will be an anonymous guy,
            // when authenticatio & authorization is done, this needs to be refactored
            // using AccountService
            var wsClient = new WebSocketClient(new User(), webSocket);
            //try
            //{
                await HandleClient(wsClient);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, $"Echo websocket client {wsClient.Participant.ID} error.");
            //    await context.Response.WriteAsync("closed");
            //    throw;
            //}
        }
        
        private async Task HandleClient(WebSocketClient webSocket)
        {
            WebSocketClientCollection.Add(webSocket);
            _logger.LogInformation($"Websocket client added.");

            WebSocketReceiveResult result;
            do
            {
                var buffer = new byte[1024 * 1];
                result = await webSocket.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text && !result.CloseStatus.HasValue)
                {
                    var messageJSON = Encoding.UTF8.GetString(buffer);
                    var message = JsonConvert.DeserializeObject<MessageDTO>(messageJSON);
                    message.AuthorID = webSocket.Participant.ID;
                    HandleMessage(message);
                }
            }
            while (!result.CloseStatus.HasValue);
            WebSocketClientCollection.Remove(webSocket);
            _logger.LogInformation($"Websocket client disconnected.");
        }

        private void HandleMessage(MessageDTO dto)
        {
            WebSocketClient? wsclient = WebSocketClientCollection.Get(dto.AuthorID);
            if (wsclient is null)
            {
                _logger.LogError($"Author {dto.AuthorID} not found.");
                return;
            }
            switch (dto.Action)
            {
                case MessageAction.JOIN:
                    if (!Guid.TryParse(dto.Content, out Guid convID))
                    {
                        _logger.LogError($"Invalid conversation ID '{dto.Content}'.");
                        return;
                    }

                    var conversation = _conversationService.GetConversationById(convID);
                    if(conversation is null)
                        _conversationService.CreateNewConversation(convID);

                    var participant = _participantService.GetParticipantById(wsclient.Participant.ID);
                    if (participant is null)
                        participant = _userService.CreateNewUser(wsclient.Participant.ID);

                    if (participant is null)
                    {
                        _logger.LogError($"Client cannot have the Bot's ID: '{Bot.GetChatBotID()}'.");
                        return;
                    }
                    
                    _conversationService.AddParticipantToConversation(participant, convID);


                    if (wsclient.ConversationID is null)
                    {
                        _logger.LogInformation($"Client '{wsclient.Participant.ID}' joined conversation with ID: '{convID}'");

                    }
                    else if (wsclient.ConversationID == convID)
                    {
                        _logger.LogWarning($"Client ID: '{wsclient.Participant.ID}' tried joining a conversation they are already in with ID: '{convID}'");
                        return;
                    }
                    else
                    {
                        _logger.LogInformation($"Client ID: '{wsclient.Participant.ID}' switched to conversation ID: '{convID}'");
                    }
                    wsclient.ConversationID = convID;
                    break;

                case MessageAction.SEND:
                    if (wsclient.ConversationID is null)
                    {
                        _logger.LogError($"Client '{dto.AuthorID}' is not in a conversation.");
                        return;
                    }
                    // TODO: add global exception handlers to notify client of bad requests
                    try
                    {  
                        _conversationService.AddMessageToConversation(dto, wsclient.ConversationID.Value);
                        _logger.LogInformation($"Saved message to the database.");
                    }
                    catch (InvalidMessageException ex)
                    {
                        _logger.LogError(ex, $"Invalid message from client '{dto.AuthorID}'. Reason: {ex.Message}");
                        return;
                    }
                    var clients = WebSocketClientCollection.GetByConversationID(wsclient.ConversationID);
                    clients.ForEach(c => c.SendMessageAsync(dto));
                    _logger.LogInformation($"Client ID: '{dto.AuthorID}' sent a message: '{dto.Content}'.");
                    //AI injection
                    if (dto.QuickSelector == QuickSelector.faq)
                    {
                      string faqAnswer = _aiClientService.getFaqAnswer(dto.Content).Result; //TODO: maybe async
                       var aiMs =new MessageDTO(){AuthorID = Bot.GetChatBotID(), Content = faqAnswer, Action = MessageAction.SEND, QuickSelector = QuickSelector.ts, Nickname = "bot"};
                       clients.ForEach(c => c.SendMessageAsync(aiMs));
                       _logger.LogInformation($"Client ID: '{aiMs.AuthorID}' sent a message: '{aiMs.Content}'.");
                    }
                    
                    break;

                case MessageAction.LEAVE:
                    if (wsclient.ConversationID is null)
                    {
                        _logger.LogError($"Client '{dto.AuthorID}' tried leaving a conversation they are not in.");
                        return;
                    }
                    _logger.LogInformation($"Client '{dto.AuthorID}' left the room '{wsclient.ConversationID}'.");
                    _conversationService.SetConversationStatus(wsclient.ConversationID.Value, ConversationStatus.RESOLVED);
                    wsclient.ConversationID = null;
                    break;

                default:
                    return;
            }
        }
    }
}