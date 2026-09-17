using Forge.Application.Common;
using Forge.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Forge.Application.Validators
{
    public class RegisterValidator
    {
        public static void Validate(RegisterRequest request)
        {
            NameValidator.Validate(request.FirstName, "First Name", true);
            NameValidator.Validate(request.MiddleName, "Middle Name", false);
            NameValidator.Validate(request.LastName, "Last Name", true);
            NameValidator.ValidateDisplayName(request.DisplayName);

            if (string.IsNullOrWhiteSpace(request.Email)) throw new ArgumentException("Email is required.");
            if (!IsValidEmail(request.Email)) throw new ArgumentException("Please provide a valid email address.");
            if (string.IsNullOrWhiteSpace(request.Password))  throw new ArgumentException("Password is required.");
            if (request.Password != request.ConfirmPassword) throw new ArgumentException("Password and confirmation password do not match.");

            PasswordValidator.Validate(request.Password);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress address = new System.Net.Mail.MailAddress(email);
                return address.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
            }
            catch {  return false; }
        }
    }
}
