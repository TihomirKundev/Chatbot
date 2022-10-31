namespace ChatBot.Exceptions;

public class UserNotFoundException : System.Exception
{
    public UserNotFoundException(string message) : base(message)
    {
    }
}