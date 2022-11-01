using ChatBot.Exceptions;
using ChatBot.Models;
using ChatBot.Models.Request;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatBot.Http;

public class FakeApiHttpClient : IFakeApiHttpClient
{
    private readonly HttpClient _httpClient;

    public FakeApiHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<User> GetUserAsync(AuthenticateRequest logInRequest)
    {
        try
        {
            var json = JsonSerializer.Serialize(logInRequest);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5019/user"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<User>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new User(userDto.ID, //trust the process
                userDto.FirstName, userDto.LastName, userDto.Email, userDto.Phone, userDto.Password, userDto.Role);
        }
        catch (Exception)
        {
            throw new UserNotFoundException("No user found");
        }
    }
}