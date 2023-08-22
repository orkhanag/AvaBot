using AvaBot.Application.Common.Clients;
using AvaBot.Application.Features.Instructions.Commands.CreateInstruction;
using Microsoft.AspNetCore.Mvc;

namespace AvaBot.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class InstructionsController : ApiControllerBase
    {
        private readonly OpenAIClient _embeddingsClient;

        public InstructionsController(OpenAIClient embeddingsClient)
        {
            _embeddingsClient = embeddingsClient;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateInstructionCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
