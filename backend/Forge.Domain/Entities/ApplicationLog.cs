using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Domain.Entities
{
    public class ApplicationLog
    {
        public Guid Id { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public Guid? UserId { get; set; }
        public string? RequestPath { get; set; }
        public string? RequestMethod { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
