using EduVault.Models.DataTransferObjects;
using EduVault.Models;
using EduVault.Repositories;

namespace EduVault.Services
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Role>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<Role> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
    }
}
