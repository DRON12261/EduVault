using EduVault.Models.DataTransferObjects;
using EduVault.Models;
using EduVault.Repositories;

namespace EduVault.Services
{
    public interface IGroupService
    {
        Task<List<Group>> GetAllAsync();
        Task<Group> GetByIdAsync(long id);
        Task<OperationResult> CreateAsync(GroupDTO groupDTO);
        Task<OperationResult> UpdateAsync(GroupDTO groupDTO);
        Task<OperationResult> DeleteById(long id);
        Task<List<Group>> GetFilteredRecordsAsync(FilterModel filters);
    }
    public class GroupService: IGroupService
    {
        private readonly IGroupRepository _repository;
        public GroupService(IGroupRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Group>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<Group> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<OperationResult> CreateAsync(GroupDTO groupDTO)
        {
            if (await _repository.GetByNameAsync(groupDTO.Name) != null)
                return OperationResult.Failed("Такая группа уже существует!", OperationStatusCode.Conflict);

            await _repository.CreateAsync(new Group(groupDTO));
            return OperationResult.Success();
        }
        public async Task<OperationResult> UpdateAsync(GroupDTO groupDTO)
        {
            if (await _repository.GetByIdAsync(groupDTO.Id) == null)
                return OperationResult.Failed("Группа с таким Id не существует!", OperationStatusCode.NotFound);

            await _repository.UpdateAsync(new Group(groupDTO));
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteById(long id)
        {
            if (await _repository.GetByIdAsync(id) == null)
            {
                return OperationResult.Failed("Группа с таким Id не существует!", OperationStatusCode.NotFound);
            }
            await _repository.DeleteAsync(id);
            return OperationResult.Success();
        }
        public async Task<List<Group>> GetFilteredRecordsAsync(FilterModel filters)
        {
            return await _repository.GetFilteredRecordsAsync(filters);
        }
    }
}
