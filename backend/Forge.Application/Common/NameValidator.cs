using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Forge.Application.Common
{
    public static class NameValidator
    {
        public static void Validate(string? value, string fieldName, bool required)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (required) { throw new ArgumentException($"{fieldName} is required."); }

                return;
            }

            var name = value.Trim();

            if (name.Length < 2 || name.Length > 50) throw new ArgumentException($"{fieldName} must be between 2 and 50 characters.");
            if (!Regex.IsMatch(name, @"^[\p{L}]+(?:[ '\-][\p{L}]+)*$")) throw new ArgumentException($"{fieldName} contains invalid characters.");
        }

        public static void ValidateDisplayName(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            string displayName = value.Trim();

            if (displayName.Length < 2 || displayName.Length > 30) throw new ArgumentException("Nickname must be between 2 and 30 characters.");
            if (!Regex.IsMatch(displayName, @"^[a-zA-Z0-9 _\-]+$")) throw new ArgumentException("Nickname contains invalid characters.");
        }
    }
}
