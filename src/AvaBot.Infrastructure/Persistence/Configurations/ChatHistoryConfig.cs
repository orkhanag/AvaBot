using AvaBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Infrastructure.Persistence.Configurations
{
    public class ChatHistoryConfig : IEntityTypeConfiguration<ChatHistory>
    {
        public void Configure(EntityTypeBuilder<ChatHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Role)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(950);

            builder.Property(x => x.TotalUsage)
                .IsRequired();
        }
    }
}
