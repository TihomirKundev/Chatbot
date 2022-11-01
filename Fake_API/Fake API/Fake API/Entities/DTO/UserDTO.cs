using FakeAPI.Enums;

namespace FakeAPI.Entities.DTO;

public class UserDTO
{
    public Guid Id {get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}