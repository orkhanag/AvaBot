using AvaBot.Application.Common.Clients;
using AvaBot.Application.Common.Interfaces;
using AvaBot.Application.Common.Models.Wrappers;
using AvaBot.Domain.Entities;
using MediatR;
using Pgvector;

namespace AvaBot.Application.Features.Instructions.Commands.CreateInstruction;
public record CreateInstructionCommand(string Input) : IRequest<EitherOne<Guid, APIError>>;


public class CreateInstructionCommandHandler : IRequestHandler<CreateInstructionCommand, EitherOne<Guid, APIError>>
{
    private readonly OpenAIClient _openAIClient;
    private readonly IApplicationDbContext _context;

    public CreateInstructionCommandHandler(OpenAIClient openAIClient, IApplicationDbContext context)
    {
        _openAIClient = openAIClient;
        _context = context;
    }

    public async Task<EitherOne<Guid, APIError>> Handle(CreateInstructionCommand request, CancellationToken cancellationToken)
    {
        var embeddings = await _openAIClient.CreateEmbeddings(request.Input);

        var vector = new Vector(embeddings.Data.FirstOrDefault().Embedding);

        var instruction = new Instruction
        {
            Value = request.Input,
            Embeddings = vector,
            TokensCount = embeddings.Usage.TotalTokens
        };

        _context.Instructions.Add(instruction);

        await _context.SaveChangesAsync(cancellationToken);

        return instruction.Id;
    }
}


