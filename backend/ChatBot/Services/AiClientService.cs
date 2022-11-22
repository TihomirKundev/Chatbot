using ChatBot.Extensions;
using ChatBot.Models.DTOs;
using ChatBot.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChatBot.Http;
using ChatBot.Http.Model;
using ChatBot.Models;

namespace ChatBot.Services;

[SingletonService]
public class AiClientService : IAiClientService
{
    private string _aiBaseURL = "http://127.0.0.1:8000";
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly IUserHttpClient _fakeApiHttpClient;
    private readonly IConversationService _conversationService;

    public AiClientService(HttpClient httpClient, IUserHttpClient fakeApiHttpClient, IConversationService conversationService)
    {
        _httpClient = httpClient;
        _fakeApiHttpClient = fakeApiHttpClient;
        _conversationService = conversationService;
        _conversationService.MessageSent += OnMessageSent;
    }

    private async void OnMessageSent(Guid conversationID, MessageDTO message)
    {
        if (message.Content is not { } content)
            return;

        if (message.QuickSelector switch {
            QuickSelector.Faq => await getFaqAnswer(content),
            QuickSelector.Order => await getOrderAnswer(message.AuthorID, content),
            _ => null
        } is { } answer)
            _conversationService.AddMessageToConversation(new MessageDTO {
                AuthorID = Bot.GetChatBotID(),
                Content = answer,
                Nickname = "Bot",
                Timestamp = DateTime.Now.ToUnixTimestamp()
            }, conversationID);
    }

    public async Task<string> getFaqAnswer(string message)
    {
        AiQuestionDTO question = new AiQuestionDTO() { question = message };
        JsonContent converted = JsonContent.Create(question);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_aiBaseURL}/faqAnswer"),
            Content = converted
        };
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var answer = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return answer;
    }
    //getorderanswer(fakeApiUserDTO, message) get the user from the httpClient to the fakeApi

    public async Task<string> getOrderAnswer(Guid userID,string message)
    {
        AiQuestionDTO question = new AiQuestionDTO() { question = message };
        FakeApiUserDTO user = _fakeApiHttpClient.GetFakeApiUserDTOByIdAsync(userID).Result;
        OrderQuestionDTO orderQuestionDTO = new OrderQuestionDTO(question, user);
        JsonContent converted = JsonContent.Create(orderQuestionDTO);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_aiBaseURL}/orderAnswer"),
            Content = converted
        };
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var answer = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return answer;
    }
}