using System.Threading.Tasks;
using ChatBot.Http.Requests;
using ChatBot.Models;
using ChatBot.Models.Request;

namespace ChatBot.Http;

public interface IFakeApiHttpClient
{
    Task<User> GetUserAsync(AuthenticateRequest request);
}