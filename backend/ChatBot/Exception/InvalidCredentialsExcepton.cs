using System;

namespace ChatBot.Auth.Exception.CustomExceptions;

public class InvalidCredentialsException : System.Exception
{
    public InvalidCredentialsException(string invalidCredentials)
    {
    }
}