using Microsoft.AspNetCore.Diagnostics;

namespace ChatBot.Auth.Helpers;

public class AppSettings
{
    public string Secret { get; private set; } = "c#isbetterthanjava";
}