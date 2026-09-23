using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.Auth
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
