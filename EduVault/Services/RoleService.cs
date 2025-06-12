using EduVault.Models.DataTransferObjects;
using EduVault.Models;
using EduVault.Repositories;

namespace EduVault.Services
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(long id);
    }
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<List<Role>> GetAllAsync()
        {
            return await _roleRepository.GetAllAsync();
        }
        public async Task<Role> GetByIdAsync(long id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }
        
    }
}
