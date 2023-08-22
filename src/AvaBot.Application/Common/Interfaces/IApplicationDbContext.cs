using AvaBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AvaBot.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Instruction> Instructions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
