using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Validators
{
    public class ProjectValidator
    {
        public static void ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Project name is required.");

            var trimmedName = name.Trim();

            if (trimmedName.Length < 2) throw new ArgumentException("Project name must be at least 2 characters.");

            if (trimmedName.Length > 100) throw new ArgumentException("Project name must not exceed 100 characters.");
        }

        public static string NormalizeName(string name)
        {
            return name.Trim();
        }

        public static string? NormalizeDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description)) return null;

            return description.Trim();
        }
    }
}
