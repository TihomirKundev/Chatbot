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
[TransientService]
public class AiClientService : IAiClientService
{
    private string _aiBaseURL = "http://127.0.0.1:8000";
    private HttpClient _httpClient = new HttpClient();
    private FakeApiHttpClient _fakeApiHttpClient;

    public AiClientService() { }

    public AiClientService(string aiBaseURL, HttpClient httpClient, FakeApiHttpClient fakeApiHttpClient)
    {
        _aiBaseURL = aiBaseURL;
        _httpClient = httpClient;
        _fakeApiHttpClient = new FakeApiHttpClient(_httpClient);

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

    public async Task<string> getOrderAnswer(Guid id,string message)
    {
        AiQuestionDTO question = new AiQuestionDTO() { question = message };
        FakeApiUserDTO user = _fakeApiHttpClient.GetFakeApiUserDTOByIdAsync(id).Result;
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