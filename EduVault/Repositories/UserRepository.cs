using EduVault.Data;
using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using System;

namespace EduVault.Repositories
{
	public interface IUserRepository
	{
		Task<User> GetByIdAsync(long id);
        Task<User> GetByLoginAsync(string login);
		Task CreateAsync(User user);
		Task UpdateAsync(User user);
		Task DeleteAsync(long id);
		Task<List<User>> GetAllAsync();
        Task<List<User>> GetFilteredRecordsAsync(FilterModel filters);
        Task<List<User>> GetUsersForGroupIdAsync(long groupId);

    }
	public class UserRepository: IUserRepository
	{
        private readonly IDbContextFactory<PostgresDBContext> _contextFactory;

		public UserRepository(IDbContextFactory<PostgresDBContext> contextFactory)
		{
            _contextFactory = contextFactory;
		}

		public async Task<User> GetByIdAsync(long id)
		{
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
			return await _context.Users.FindAsync(id);
		}

        public async Task<User> GetByLoginAsync(string login)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Users.FirstOrDefaultAsync(user => user.Login == login);
        }
        public async Task CreateAsync(User user)
		{
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(User user)
		{
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            _context.Users.Update(user);
			await _context.SaveChangesAsync();
		}
		public async Task<List<User>> GetAllAsync()
		{
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Users
                .AsNoTracking()
                .OrderBy(r => r.Id)
                .ToListAsync();
		}
		public async Task DeleteAsync(long id)
		{
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            var user = await _context.Users.FindAsync(id);
			if (user != null)
			{
				_context.Users.Remove(user);
				await _context.SaveChangesAsync();
			}
		}
        public async Task<List<User>> GetFilteredRecordsAsync(FilterModel filters)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            var query = _context.Users.AsQueryable();

            if (filters.Id.HasValue)
            {
                query = query.Where(r => r.Id == filters.Id.Value);
            }
            if (!string.IsNullOrEmpty(filters.Name))
            {
                query = query.Where(r => r.Name.Contains(filters.Name));
            }
            if (!string.IsNullOrEmpty(filters.Login))
            {
                query = query.Where(r => r.Login.Contains(filters.Login));
            }
            if (filters.RoleId.HasValue)
            {
                query = query.Where(r => r.RoleId == filters.RoleId.Value);
            }

            return await query
                .OrderBy(r => r.Id)
                .Select(r => r)
                .ToListAsync();
        }
        public async Task<List<User>> GetUsersForGroupIdAsync(long groupId)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Users
                .ToListAsync();//пока просто выводит всех пользователей
        }
    }
}
