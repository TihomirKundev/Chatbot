namespace ChatBot.Exceptions;

public class DuplicateEmailException : System.Exception
{
    public DuplicateEmailException(string exceptionMessage) : base(exceptionMessage)
    {
    }
}