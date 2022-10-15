using System.ComponentModel.DataAnnotations;

namespace ChatBot.Models.DTOs;

public class AuthenticateRequest
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
        
}