using ChatBot.Models;
using ChatBot.Models.Request;
using System.Threading.Tasks;

namespace ChatBot.Http;

public interface IFakeApiHttpClient
{
    Task<User> GetUserAsync(AuthenticateRequest request);
}