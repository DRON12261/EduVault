using EduVault.Data;
using EduVault.Models;
using Microsoft.EntityFrameworkCore;

namespace EduVault.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetByIdAsync(int id);
        Task<List<Role>> GetAllAsync();
    }
    public class RoleRepository: IRoleRepository
    {
        private readonly IDbContextFactory<PostgresDBContext> _contextFactory;
        public RoleRepository(IDbContextFactory<PostgresDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Role> GetByIdAsync(int id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Roles.FindAsync(id);
        }
        public async Task<List<Role>> GetAllAsync()
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Roles
                .AsNoTracking()
                .OrderBy(r => r.Id)
                .ToListAsync();
        }
    }
}
