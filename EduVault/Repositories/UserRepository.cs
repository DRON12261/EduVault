using EduVault.DBClasses;
using System;

namespace EduVault.Repositories
{
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
			return _context.Users.ToList();
		}
		public async Task DeleteAsync(int id)
		{
			
			await _context.SaveChangesAsync();
		}
	}
}
