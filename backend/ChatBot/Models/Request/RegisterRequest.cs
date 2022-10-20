using System.ComponentModel.DataAnnotations;

namespace ChatBot.Models.DTOs;

public class RegisterRequest
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
   
    [Required]
    public string Password { get; set; }

    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Role { get; set; }
   
    [Required]
    public string Phone { get; set; }
}