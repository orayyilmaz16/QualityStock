using QualityStock.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace QualityStock.Domain.Entities
{
    public sealed class Category : AuditableEntity
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }

        private Category() { } // EF

        public Category(string name, string? description = null)
        {
            SetName(name);
            Description = description;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty.", nameof(name));

            Name = name.Trim();
        }

        public void SetDescription(string? description) => Description = description?.Trim();
    }
}
