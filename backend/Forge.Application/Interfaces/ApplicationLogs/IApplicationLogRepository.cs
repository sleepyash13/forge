using Forge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.ApplicationLogs
{
    public interface IApplicationLogRepository
    {
        Task AddAsync(ApplicationLog log);
    }
}
