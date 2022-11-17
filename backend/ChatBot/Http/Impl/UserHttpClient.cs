using ChatBot.Exceptions;
using ChatBot.Models;
using ChatBot.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatBot.Models.DTOs;
using Fake_API.Entities.DTO;

namespace ChatBot.Http;

public class UserHttpClient : IUserHttpClient
{
    private readonly HttpClient _httpClient;

    public UserHttpClient(HttpClient httpClient)
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
                RequestUri = new Uri("http://localhost:5019/user/login"),
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

    public async Task<ISet<User>> GetAllAsync()
    {
        try
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5019/user/all")
            };
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var usersDto = JsonSerializer.Deserialize<List<User>>(responseContent,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
            
            List<User> users = new List<User>();
            
            usersDto.ForEach(item => users.Add(new User(item.ID, //trust the process
                item.FirstName, item.LastName, item.Email, item.Phone, item.Password, item.Role)));
            
            return new HashSet<User>(users);
        }
        catch (Exception e)
        {
            throw new UserNotFoundException("No users found");
        }
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        try
        {
            EmailDto emailDto = new EmailDto(email);
            
            var json = JsonSerializer.Serialize(emailDto);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5019/user/email"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<User>(responseContent,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            return new User(userDto.ID, //trust the process
                userDto.FirstName, userDto.LastName, userDto.Email, userDto.Phone, userDto.Password, userDto.Role);
        }
        catch (Exception e)
        {
            throw new UserNotFoundException("No user found");
        }
    }
    
    public async Task<User> GetByIdAsync(Guid id)
    {
        try
        {
            GuidDto guidDto = new GuidDto(id);
            
            var json = JsonSerializer.Serialize(guidDto);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5019/user/id"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<User>(responseContent,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            return new User(userDto.ID, //trust the process
                userDto.FirstName, userDto.LastName, userDto.Email, userDto.Phone, userDto.Password, userDto.Role);
        }
        catch (Exception e)
        {
            throw new UserNotFoundException("No user found");
        }
    }

    public async Task<FakeApiUserDTO> GetFakeApiUserDTOByIdAsync(Guid id)
    {
        try
        {
            GuidDto guidDto = new GuidDto(id);

            var json = JsonSerializer.Serialize(guidDto);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:5019/user/id"),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var userDto = JsonSerializer.Deserialize<FakeApiUserDTO>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return new FakeApiUserDTO(userDto.Id, userDto.FirstName, userDto.LastName, userDto.Email, userDto.Phone, userDto.Password, userDto.Role
            , userDto.Company,userDto.Orders);
        }
        catch (Exception e)
        {
            throw new UserNotFoundException("No user found");
        }
    }
}