using System.Threading.Tasks;
using ChatBot.Models;
using ChatBot.Models.DTOs;

namespace ChatBot.Http.Model
{
    public record OrderQuestionDTO(
        AiQuestionDTO question,
        FakeApiUserDTO user
    );
    
}
