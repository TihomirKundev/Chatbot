using System;

namespace ChatBot.Exceptions;

public class TokenNotFoundException : Exception
{
    public TokenNotFoundException(string message) : base(message) { }

    public TokenNotFoundException() : base() { }

}