using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using bCrypt = BCrypt.Net.BCrypt;

namespace EduVault.Services
{
    public interface IUserService
    {
        Task<OperationResult> CheckSystemUser();
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(long id);
        Task<OperationResult> CreateAsync(UserDTO userDTO);
        Task<OperationResult> UpdateAsync(UserDTO userDTO);
        Task<OperationResult> DeleteById(long id);
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
            await _userRepository.AddAsync(new User("testUser", "test", "test", 1));
            return OperationResult.Success();
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(long id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task<OperationResult> CreateAsync(UserDTO userDTO)
        {
            if (await _userRepository.GetByLoginAsync(userDTO.Login) != null)
                return OperationResult.Failed("Логин занят", OperationStatusCode.Conflict);

            userDTO.Password = bCrypt.HashPassword(userDTO.Password);
            await _userRepository.AddAsync(new User(userDTO));
            return OperationResult.Success();
        }
        public async Task<OperationResult> UpdateAsync(UserDTO userDTO)
        {
            if (await _userRepository.GetByIdAsync(userDTO.Id) == null)
                return OperationResult.Failed("Пользователя с таким Id не существует!", OperationStatusCode.NotFound);

            await _userRepository.UpdateAsync(new User(userDTO));
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteById(long id)
        {
            if (await _userRepository.GetByIdAsync(id) == null)
            {
                return OperationResult.Failed("Пользователя с таким Id не существует", OperationStatusCode.NotFound);
            }
            await _userRepository.DeleteAsync(id);
            return OperationResult.Success();
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
