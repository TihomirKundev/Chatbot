using System.Threading.Tasks;

namespace ChatBot.Services.Interfaces;

public interface IAiClientService
{
    Task<string> getFaqAnswer(string message);
}