using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Forge.Application.Validators
{
    public class PasswordValidator
    {
        public static void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("Password is required.");

            if (password.Length < 12) throw new ArgumentException("Password must be at least 12 characters.");

            if (!Regex.IsMatch(password, "[A-Z]")) throw new ArgumentException("Password must contain at least one uppercase letter.");

            if (!Regex.IsMatch(password, "[a-z]")) throw new ArgumentException("Password must contain at least one lowercase letter.");

            if (!Regex.IsMatch(password, "[0-9]")) throw new ArgumentException("Password must contain at least one number.");

            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]")) throw new ArgumentException("Password must contain at least one special character.");
        }
    }
}
