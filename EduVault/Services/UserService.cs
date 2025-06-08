using EduVault.Models;
using EduVault.Repositories;
using bCrypt = BCrypt.Net.BCrypt;

namespace EduVault.Services
{
    public interface IUserService
    {
        Task<OperationResult> CheckSystemUser();
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(long id);
        Task<OperationResult> CreateUserAsync(UserCreationDto dto);
        Task<OperationResult> DeleteUserById(long id);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<OperationResult> CheckSystemUser()
        {
            if(await _userRepository.GetByLoginAsync("test") != null)
            {
                return OperationResult.Failed("Системный пользователь уже создан!", OperationStatusCode.Conflict);
            }
            await _userRepository.AddAsync(new User(new UserCreationDto("testUser", "test", "test")));
            return OperationResult.Success();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task<OperationResult> CreateUserAsync(UserCreationDto dto)
        {
            // Валидация
            if (await _userRepository.GetByLoginAsync(dto.Login) != null)
                return OperationResult.Failed("Логин занят", OperationStatusCode.Conflict);

            // Создание entity
            var user = new User(dto);

            await _userRepository.AddAsync(user);
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteUserById(long id)
        {
            if (await _userRepository.GetByIdAsync(id) == null)
            {
                return OperationResult.Failed("Пользователя с таким Id не существует", OperationStatusCode.NotFound);
            }
            await _userRepository.DeleteAsync(id);
            return OperationResult.Success();
        }
    }
    public class UserCreationDto
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        //public UserType UserType { get; set; }
        //public bool SendConfirmationEmail { get; set; } = true;
        public UserCreationDto(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = bCrypt.HashPassword(password);
        }
    }

    public class OperationResult
    {
        public bool Succeeded { get; init; }
        public string ErrorMessage { get; init; }
        public OperationStatusCode StatusCode { get; init; }

        public static OperationResult Success()
        => new() { Succeeded = true, StatusCode = OperationStatusCode.Success };

        public static OperationResult Failed(string error, OperationStatusCode status)
            => new() { Succeeded = false, ErrorMessage = error, StatusCode = status };
    }
    public enum OperationStatusCode
    {
        Success = 0,
        NotFound = 1,
        Conflict = 2,
        InternalError = 3
    }
}
