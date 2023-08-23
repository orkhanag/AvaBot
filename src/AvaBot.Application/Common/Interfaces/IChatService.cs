using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Application.Common.Interfaces;
public interface IChatService
{
    public Task TerminateChat(Guid userId, CancellationToken cancellationToken);
}
