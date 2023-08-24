using AvaBot.Application.Common.Clients;
using AvaBot.Application.Common.Interfaces;
using AvaBot.Application.Common.Models.Wrappers;
using AvaBot.Domain.Entities;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pgvector;

namespace AvaBot.Application.Features.Chat.Commands.CreateChatCompletion;
public record CreateChatCompletionCommand(string Input) : IRequest<EitherOne<string, APIError>>;


public class CreateChatCompletionCommandHandler : IRequestHandler<CreateChatCompletionCommand, EitherOne<string, APIError>>
{
    private readonly OpenAIClient _openAIClient;
    private readonly IApplicationDbContext _context;
    private readonly IChatService _chatService;
    private readonly IDapperDbContext _dapperContext;

    private readonly string _baseInstruction = @"""You are a helpful assistant in an e-commerce platform.
        Your main task is to answer customer questions based on instructions provided to you.
        Don’t answer any unrelated questions, only generate your answers according to the data provided to you.
        Don’t use any external source to generate answers. Keep answers simple and short as much as possible.
        You answer questions in the language the question asked.
        If someone asks a question unrelated to the data provided to you simply generate this answer:
        “Sorry, I don’t have enough information to answer your question. Do you want me to connect you with our customer representative?”""";

    public CreateChatCompletionCommandHandler(
        OpenAIClient openAIClient,
        IApplicationDbContext context,
        IChatService chatService,
        IDapperDbContext dapperContext)
    {
        _openAIClient = openAIClient;
        _context = context;
        _chatService = chatService;
        _dapperContext = dapperContext;
    }

    public async Task<EitherOne<string, APIError>> Handle(CreateChatCompletionCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse("7aa1a9d7-161c-4891-aa65-623bfc6977b6");

        if (request.Input == "Done")
        {
            _chatService.TerminateChat(userId, cancellationToken);
            return "Great! If you have any more questions in the future, feel free to ask. Have a wonderful day!";
        }


        var inputEmbeddingsResponse = await _openAIClient.CreateEmbeddings(request.Input);

        var embedding = new Vector(inputEmbeddingsResponse.Data.FirstOrDefault().Embedding);

        //using var conn = _dapperContext.CreateConnection();

        //var instructions = conn.Query<Instruction>("""SELECT * FROM "Instructions" i ORDER BY i."Embeddings" <-> @embedding LIMIT 1""", new { embedding });

        var instructions = _context.Instructions.FromSql($"""SELECT * FROM "Instructions" i ORDER BY i."Embeddings" <-> {embedding} LIMIT 1""");

        var messages = new List<Message>
        {
            new Message { Role = "system", Content = _baseInstruction },
        };

        foreach (var instruction in instructions)
            messages.Add(new Message { Role = "system", Content = instruction.Value });


        var userMessage = new ChatHistory
        {
            UserId = userId,
            IsActive = true,
            Role = "user",
            Content = request.Input,
            TotalUsage = 0,
        };

        _context.ChatHistories.Add(userMessage);

        var chatContext = _context.ChatHistories.AsNoTracking().Where(x => x.UserId == userId && x.IsActive).OrderBy(x => x.Created);


        foreach (var item in chatContext)
            messages.Add(new Message { Role = item.Role, Content = item.Content });

        messages.Add(new Message { Role = "user", Content = request.Input });

        var result = await _openAIClient.CreateChatCompletion(messages, false);

        var assistantMessage = new ChatHistory
        {
            UserId = userId,
            IsActive = true,
            Role = result.Choices.FirstOrDefault().Message.Role,
            Content = result.Choices.FirstOrDefault().Message.Content,
            TotalUsage = result.Usage.TotalTokens
        };

        _context.ChatHistories.Add(assistantMessage);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Choices.FirstOrDefault().Message.Content;
    }
}