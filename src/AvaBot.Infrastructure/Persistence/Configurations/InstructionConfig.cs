using AvaBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Infrastructure.Persistence.Configurations
{
    public class InstructionConfig : IEntityTypeConfiguration<Instruction>
    {
        public void Configure(EntityTypeBuilder<Instruction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.Embeddings)
                .HasColumnType("vector")
                .IsRequired();

            builder.Property(x => x.TokensCount)
                .IsRequired();
        }
    }
}
