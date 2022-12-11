using ChatBot.Models;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatBot.Services.Interfaces
{
	public interface IWebSocketService
	{
		Task HandleConnection(User user, WebSocket webSocket);
	}
}
