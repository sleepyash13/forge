using Forge.Application.Interfaces.ApplicationLogs;
using Forge.Domain.Entities;
using Forge.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Infrastructure.Repositories
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly ForgeDbContext _dbContext;

        public ApplicationLogRepository(ForgeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ApplicationLog log)
        {
            await _dbContext.ApplicationLogs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
        }
    }
}
