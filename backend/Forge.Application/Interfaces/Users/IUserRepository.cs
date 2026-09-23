using Forge.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Application.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
