namespace Fake_API.Exception;

public class UserNotFoundException : System.Exception
{
    public UserNotFoundException() : base() { }
    public UserNotFoundException(string message) : base(message) { }
}