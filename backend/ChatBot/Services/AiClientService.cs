using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ChatBot.Extensions;
using ChatBot.Models.DTOs;
using Newtonsoft.Json;

namespace ChatBot.Services;
[TransientService]
public class AiClientService : IAiClientService
{
    private string _aiBaseURL = "http://127.0.0.1:8000";
    private HttpClient _httpClient = new HttpClient();
    
    public AiClientService(){}

    public  async Task<string> getFaqAnswer(string message)
    {
        AiQuestionDTO question = new AiQuestionDTO(){question = message};
        JsonContent converted = JsonContent.Create(question);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get, RequestUri = new Uri($"{_aiBaseURL}/faqAnswer"), Content = converted
        };
        var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var answer = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return answer;
    }
}