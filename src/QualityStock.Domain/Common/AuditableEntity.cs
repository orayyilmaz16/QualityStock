using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Domain.Common
{
    public abstract class AuditableEntity : Entity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
