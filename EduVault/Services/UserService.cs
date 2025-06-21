using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using static NuGet.Packaging.PackagingConstants;
using bCrypt = BCrypt.Net.BCrypt;

namespace EduVault.Services
{
    public interface IUserService
    {
        Task<OperationResult> CheckSystemUser();
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(long id);
        Task<User> GetByLoginAsync(string login);
        Task<OperationResult> CreateAsync(UserDTO userDTO);
        Task<OperationResult> UpdateAsync(UserDTO userDTO);
        Task<OperationResult> DeleteById(long id);
        Task<List<User>> GetFilteredRecordsAsync(FilterModel filters);
        Task<List<UserDTO>> GetUsersForGroupAsync(long groupId);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<OperationResult> CheckSystemUser()
        {
            if(await _repository.GetByLoginAsync("test") != null)
            {
                return OperationResult.Failed("Системный пользователь уже создан!", OperationStatusCode.Conflict);
            }
            await _repository.CreateAsync(new User("testUser", "test", bCrypt.HashPassword("test"), 1));
            return OperationResult.Success();
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<User> GetByLoginAsync(string login)
        {
            return await _repository.GetByLoginAsync(login);
        }
        public async Task<OperationResult> CreateAsync(UserDTO userDTO)
        {
            if (await _repository.GetByLoginAsync(userDTO.Login) != null)
                return OperationResult.Failed("Логин занят", OperationStatusCode.Conflict);

            userDTO.Password = bCrypt.HashPassword(userDTO.Password);
            await _repository.CreateAsync(new User(userDTO));
            return OperationResult.Success();
        }
        public async Task<OperationResult> UpdateAsync(UserDTO userDTO)
        {
            User dbUser = await _repository.GetByIdAsync(userDTO.Id);
            if (dbUser == null)
                return OperationResult.Failed("Пользователя с таким Id не существует!", OperationStatusCode.NotFound);
            if (userDTO.Login != dbUser.Login && await _repository.GetByLoginAsync(userDTO.Login) != null)
                return OperationResult.Failed("Логин занят", OperationStatusCode.Conflict);

            await _repository.UpdateAsync(new User(userDTO));
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteById(long id)
        {
            if (await _repository.GetByIdAsync(id) == null)
            {
                return OperationResult.Failed("Пользователя с таким Id не существует", OperationStatusCode.NotFound);
            }
            await _repository.DeleteAsync(id);
            return OperationResult.Success();
        }
        public async Task<List<User>> GetFilteredRecordsAsync(FilterModel filters)
        {
            return await _repository.GetFilteredRecordsAsync(filters);
        }
        public async Task<List<UserDTO>> GetUsersForGroupAsync(long groupId)
        {
            List<User> users = await _repository.GetUsersForGroupIdAsync(groupId);
            return users.Select(u=>new UserDTO(u)).ToList();
        }
    }

    public class OperationResult
    {
        public bool Succeeded { get; init; }
        public string Message { get; init; }
        public OperationStatusCode StatusCode { get; init; }

        public static OperationResult Success(string message="")
        => new() { Succeeded = true, Message = message,  StatusCode = OperationStatusCode.Success };

        public static OperationResult Failed(string error, OperationStatusCode status)
            => new() { Succeeded = false, Message = error, StatusCode = status };
    }
    public enum OperationStatusCode
    {
        Success = 0,
        NotFound = 1,
        Conflict = 2,
        InternalError = 3
    }
}
