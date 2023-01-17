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
using Newtonsoft.Json;

namespace ChatBot.Services;

[SingletonService]
public class AiClientService : IAiClientService
{
    private string _aiBaseURL = "http://145.93.76.90:8000";
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

        string? classification = message.QuickSelector switch
        {
            QuickSelector.Auto => await getClassification(content),
            QuickSelector.Faq => "faq",
            QuickSelector.Order => "order",
            _ => null
        };
    if (classification is null)
        return;

        if (classification switch {
            "faq" => await getFaqAnswer(content),
            "order" => await getOrderAnswer(message.AuthorID, content),
            _ => null
        } is { } answer)
            _conversationService.AddMessageToConversation(new MessageDTO {
                AuthorID = Bot.GetChatBotID(),
                Content = answer,
                Nickname = "Bot",
                Timestamp = DateTime.Now.ToUnixTimestamp()
            }, conversationID);
    }

    private async Task<string> getClassification(string message)
    {
        AiQuestionDTO question = new AiQuestionDTO() { question = message };
        JsonContent converted = JsonContent.Create(question);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_aiBaseURL}/modelClassification/"),
            Content = converted
        };
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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
        string cleaned = answer.Replace("\\n", " ");
        return cleaned;
    }
    //getorderanswer(fakeApiUserDTO, message) get the user from the httpClient to the fakeApi

    public async Task<string> getOrderAnswer(Guid userID,string message)
    {
        AiQuestionDTO question = new AiQuestionDTO() { question = message };
        FakeApiUserDTO user = _fakeApiHttpClient.GetFakeApiUserDTOByIdAsync(userID).Result;
        AIBEuserDTO convUser = new AIBEuserDTO(user.Id,user.FirstName,user.LastName,user.Email,user.Password,user.Phone,user.Role,user.Company.Name,user.Orders);
        OrderQuestionDTO orderQuestionDTO = new OrderQuestionDTO(question.question, convUser);
        JsonContent converted = JsonContent.Create(orderQuestionDTO);
        string b = System.Text.Json.JsonSerializer.Serialize(orderQuestionDTO);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_aiBaseURL}/orderAnswer"),
            Content = converted,
           
        };
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var answer = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        string cleaned = answer.Replace("\\n", "");
        
        return cleaned;
    }
}
