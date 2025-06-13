using EduVault.Data;
using EduVault.Models;
using Microsoft.EntityFrameworkCore;

namespace EduVault.Repositories
{
    public interface IGroupRepository
    {
        Task<Group> GetByIdAsync(long id);
        Task<Group> GetByNameAsync(string name);
        Task CreateAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(long id);
        Task<List<Group>> GetAllAsync();
    }
    public class GroupRepository: IGroupRepository
    {
        private readonly IDbContextFactory<PostgresDBContext> _contextFactory;
        public GroupRepository(IDbContextFactory<PostgresDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Group> GetByIdAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Groups.FindAsync(id);
        }
        public async Task<Group> GetByNameAsync(string name)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Groups.FirstOrDefaultAsync(group => group.Name == name);
        }
        public async Task CreateAsync(Group group)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Group group)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Group>> GetAllAsync()
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Groups
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task DeleteAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }
    }
}
