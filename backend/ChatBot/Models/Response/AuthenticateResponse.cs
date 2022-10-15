using System;

namespace ChatBot.Models.Response;

public class AuthenticateResponse
{
    //now idk what u guys need in front end
    //modify this class to what u need id (service as well)
    
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Token { get; set; }
    
    public AuthenticateResponse(User user, string token)
    {
        Id = user.ID.ToString();
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Role = user.Role.ToString();
        Token = token;
    }
}