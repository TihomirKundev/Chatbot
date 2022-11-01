using System;

namespace ChatBot.Models;

// Named 'bot' because 'chatbot' is the name of project (sadly)
// Maybe needs refactoring
public class Bot : IParticipant
{
    private static readonly Guid _id = new("{00000000-0000-0000-0000-000000000001}");

    public Guid ID => _id;

    public static Guid GetChatBotID() => _id;
}
