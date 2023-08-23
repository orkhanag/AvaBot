using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaBot.Domain.Entities
{
    public class ChatHistory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public string Content { get; set; }
        public int? TotalUsage { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
