using Forge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.Projects
{
    public interface IProjectAuthorizationService
    {
        Task<bool> HasPermissionAsync(Guid userId, Guid projectId, ProjectPermission permission);
    }
}
