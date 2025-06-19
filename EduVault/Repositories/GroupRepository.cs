using EduVault.Data;
using EduVault.Models;
using EduVault.Models.DataTransferObjects;
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
        Task<List<Group>> GetFilteredRecordsAsync(FilterModel filters);
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
                .OrderBy(r => r.Id)
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
        public async Task<List<Group>> GetFilteredRecordsAsync(FilterModel filters)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            var query = _context.Groups.AsQueryable();

            if (filters.Id.HasValue)
            {
                query = query.Where(r => r.Id == filters.Id.Value);
            }
            if (!string.IsNullOrEmpty(filters.Name))
            {
                query = query.Where(r => r.Name.Contains(filters.Name));
            }

            return await query
                .OrderBy(r => r.Id)
                .Select(r => r)
                .ToListAsync();
        }
    }
}
