namespace Fake_API.DTOs
{
    public record LoginCredentialsDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
