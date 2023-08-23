using AvaBot.Application.Features.Chat.Commands.CreateChatCompletion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvaBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateChatCompletion([FromBody] CreateChatCompletionCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
