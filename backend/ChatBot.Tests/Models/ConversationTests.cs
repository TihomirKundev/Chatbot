using ChatBot.Models;
using ChatBot.Models.Enums;
namespace ChatBot.Tests.Models;

public class ConversationTests
{
    [Test]
    public void SortedSet_FirstMessage()
    {
        var author = new User(Guid.NewGuid(), "Namey", "Name", "email", "phone", "password", 0);
        var time = DateTime.Now;
        var convo = new Conversation(Guid.NewGuid());

        convo.AddMessage(new Message(author, "b", time.AddSeconds(2)));
        convo.AddMessage(new Message(author, "a", time));
        convo.AddMessage(new Message(author, "b", time.AddSeconds(1)));
        convo.EndConversation(ConversationStatus.RESOLVED);

        var firstMessage = convo.Messages.MinBy(m => m.Timestamp);

        Assert.IsNotNull(firstMessage);

        Assert.That(convo.StartTime, Is.EqualTo(firstMessage.Timestamp));
    }
    [Test]
    public void SortedSet_LastMessage_Ended_Conversation()
    {
        var author = new User(Guid.NewGuid(), "Namey", "Name", "email", "phone", "password", 0);

        var time = DateTime.Now;
        var convo = new Conversation(Guid.NewGuid());

        convo.AddMessage(new Message(author, "b", time.AddSeconds(2)));
        convo.AddMessage(new Message(author, "a", time));
        convo.AddMessage(new Message(author, "b", time.AddSeconds(1)));
        convo.EndConversation(ConversationStatus.RESOLVED);

        var lastMessage = convo.Messages.MaxBy(m => m.Timestamp);

        Assert.IsNotNull(lastMessage);

        Assert.That(convo.EndTime, Is.EqualTo(lastMessage.Timestamp));

    }
    [Test]
    public void SortedSet_LastMessage_Ongoing_Conversation()
    {
        var author = new User(Guid.NewGuid(), "Namey", "Name", "email", "phone", "password", 0);
        var time = DateTime.Now;
        var convo = new Conversation(Guid.NewGuid());

        convo.AddMessage(new Message(author, "b", time.AddSeconds(2)));
        convo.AddMessage(new Message(author, "a", time));
        convo.AddMessage(new Message(author, "b", time.AddSeconds(1)));

        Assert.IsNull(convo.EndTime);
    }
}
