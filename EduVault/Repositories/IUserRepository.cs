using EduVault.DBClasses;

namespace EduVault.Repositories
{
	public interface IUserRepository
	{
		Task<User> GetByIdAsync(int id);
		Task AddAsync(User user);
		Task UpdateAsync(User user);
		Task DeleteAsync(int id);
		Task<List<User>> GetAllAsync();
	}
}
