using Forge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Infrastructure.Persistence
{
    public class ForgeDbContext : DbContext
    {
        public ForgeDbContext(DbContextOptions<ForgeDbContext> options)
        : base(options)
        {
        }

        public DbSet<User>  Users => Set<User>();
    }
}
