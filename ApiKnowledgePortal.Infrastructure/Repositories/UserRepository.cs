using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiKnowledgePortal.Application.Abstractions.Persistence;
using ApiKnowledgePortal.Domain.Users;
using ApiKnowledgePortal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiKnowledgePortal.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApiKnowledgePortalDbContext _dbContext;

        public UserRepository(ApiKnowledgePortalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
            if (entity != null)
            {
                _dbContext.Users.Remove(entity);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbContext.Users.Update(user);
            await Task.CompletedTask;
        }

        public IQueryable<User> Query() => _dbContext.Users.AsQueryable();
    }
}