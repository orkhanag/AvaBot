using System.Data;

namespace AvaBot.Application.Common.Interfaces;

public interface IDapperDbContext
{
    public IDbConnection CreateConnection();
}