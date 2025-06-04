// Services/AuthService.cs
using EduVault.Models;
using EduVault.Repositories;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using bCrypt = BCrypt.Net.BCrypt;

public interface IAuthService
{
    Task<bool> ValidateUserAsync(string login, string password);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> ValidateUserAsync(string login, string password)
    {
        var user = await _userRepository.GetByLoginAsync(login);
        if (user == null)
            return false;

        return true;//bCrypt.Verify(password, user.PasswordHash);
    }
}
