using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}
