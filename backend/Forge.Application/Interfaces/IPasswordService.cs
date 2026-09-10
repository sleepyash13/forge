using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
