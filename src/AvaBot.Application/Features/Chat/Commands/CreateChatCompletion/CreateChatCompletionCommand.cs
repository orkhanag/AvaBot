using AvaBot.Application.Common.Clients;
using AvaBot.Application.Common.Interfaces;
using AvaBot.Application.Common.Models.Wrappers;
using AvaBot.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Application.Features.Chat.Commands.CreateChatCompletion;
public record CreateChatCompletionCommand(string Input) : IRequest<EitherOne<string, APIError>>;


public class CreateChatCompletionCommandHandler : IRequestHandler<CreateChatCompletionCommand, EitherOne<string, APIError>>
{
    private readonly OpenAIClient _openAIClient;
    private readonly IApplicationDbContext _context;
    private readonly IChatService _chatService;

    public CreateChatCompletionCommandHandler(OpenAIClient openAIClient, IApplicationDbContext context, IChatService chatService)
    {
        _openAIClient = openAIClient;
        _context = context;
        _chatService = chatService;
    }

    public async Task<EitherOne<string, APIError>> Handle(CreateChatCompletionCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse("7aa1a9d7-161c-4891-aa65-623bfc6977b6");

        if (request.Input == "Done")
        {
            _chatService.TerminateChat(userId, cancellationToken);
            return "Great! If you have any more questions in the future, feel free to ask. Have a wonderful day!";
        }

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

        var messages = new List<Message>();

        foreach (var item in chatContext)
            messages.Add(new Message { Role = item.Role, Content = item.Content });

        messages.Add(new Message { Role = "user", Content = request.Input });

        var result = await _openAIClient.CreateChatCompletion(messages);

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