
using Pgvector;

namespace AvaBot.Domain.Entities;
public class Instruction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Value { get; set; }
    public int TokensCount { get; set; }
    public Vector Embeddings { get; set; }
}
