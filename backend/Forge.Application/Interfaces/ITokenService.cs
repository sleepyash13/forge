using Forge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
