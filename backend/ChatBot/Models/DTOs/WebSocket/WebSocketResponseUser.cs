namespace ChatBot.Models.DTOs.WebSocket
{
    public class WebSocketResponseUser : WebSocketResponse
    {
        public WebSocketResponseUser(User user) : base("user")
        {
            ID = user.ID.ToString();
            Name = user.FirstName + " " + user.LastName;
        }

        public string ID { get; }
        public string Name { get; }
    }
}
