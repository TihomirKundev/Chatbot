using System;

namespace ChatBot.Auth;

[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{
    //skips authorization if the action method is decorated with [AllowAnonymous].
}