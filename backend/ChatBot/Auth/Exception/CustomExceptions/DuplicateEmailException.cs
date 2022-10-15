namespace ChatBot.Auth.Exception.CustomExceptions;

public class DuplicateEmailException : System.Exception
{
    public DuplicateEmailException(string exceptionMessage) : base(exceptionMessage)
    {
    }
}