using EduVault.Data;
using EduVault.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EduVault.Repositories
{
	public interface IUserRepository
	{
		Task<User> GetByIdAsync(int id);
        Task<User> GetByLoginAsync(string login);
		Task AddAsync(User user);
		Task UpdateAsync(User user);
		Task DeleteAsync(int id);
		Task<List<User>> GetAllAsync();
	}
	public class UserRepository: IUserRepository
	{
		private readonly AppDbContext _context;

		public UserRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<User> GetByIdAsync(int id)
		{
			return await _context.Users.FindAsync(id);
		}

        public async Task<User> GetByLoginAsync(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Login == login);
        }
        public async Task AddAsync(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
		}
		public async Task<List<User>> GetAllAsync()
		{
			return await _context.Users.ToListAsync();
		}
		public async Task DeleteAsync(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user != null)
			{
				_context.Users.Remove(user);
				await _context.SaveChangesAsync();
			}
		}
	}
}
