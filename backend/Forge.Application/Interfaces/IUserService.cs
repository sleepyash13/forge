using Forge.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileResponse> GetCurrentUserAsync(Guid userId);
    }
}
