// Services/AuthService.cs
using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using bCrypt = BCrypt.Net.BCrypt;

namespace EduVault.Services
{
    public interface IAuthService
    {
        Task<Role> ValidateUserAsync(AuthDTO authDTO);
    }
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<Role> ValidateUserAsync(AuthDTO authDTO)
        {
            User user = await _userRepository.GetByLoginAsync(authDTO.Login);
            if (user == null || !bCrypt.Verify(authDTO.Password, user.PasswordHash))
                return null;

            return await _roleRepository.GetByIdAsync(user.RoleId);
        }
    }
}
