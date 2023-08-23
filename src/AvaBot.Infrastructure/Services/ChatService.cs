using AvaBot.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly IApplicationDbContext _context;

        public ChatService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task TerminateChat(Guid userId, CancellationToken cancellationToken)
        {
            var messages = _context.ChatHistories.Where(x => x.UserId == userId && x.IsActive);

            foreach (var message in messages)
                message.IsActive = false;

            _context.ChatHistories.UpdateRange(messages);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
