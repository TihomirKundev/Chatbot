using System;

namespace ChatBot.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string invalidCredentials)
    {
    }
}