using System.Threading.Tasks;

namespace ChatBot.Services;

public interface IAiClientService
{
    Task<string> getFaqAnswer(string message);
}