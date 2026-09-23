using Forge.Application.DTOs.Users;
using Forge.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserProfileResponse> GetCurrentUserAsync(Guid userId);
        Task<UserProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileRequest request);
        Task ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
    }
}
