using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using NuGet.Protocol.Core.Types;

namespace EduVault.Services
{
    public interface IFileTypeFieldService
    {
        Task<FileTypeField> GetByIdAsync(long id);
        Task<OperationResult> DeleteByFileTypeIdAsync(long fileTypeId);
        Task<List<FileTypeFieldDTO>> GetFieldsForFileTypeAsync(long fileTypeId);

        Task<long> CreateAsync(FileTypeFieldDTO fieldDto);
        Task<OperationResult> UpdateAsync(FileTypeFieldDTO fieldDto);
        Task UpdateFieldsForFileTypeAsync(long fileTypeId, List<FileTypeFieldDTO> fields);
    }
    public class FileTypeFieldService: IFileTypeFieldService
    {
        private readonly IFileTypeFieldRepository _repository;
        public FileTypeFieldService(IFileTypeFieldRepository fileTypeFiledRepository)
        {
            _repository = fileTypeFiledRepository;
        }
        public async Task<FileTypeField> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<OperationResult> DeleteByFileTypeIdAsync(long fileTypeId)
        {
            return await _repository.DeleteByFileTypeIdAsync(fileTypeId);
        }
        public async Task<List<FileTypeFieldDTO>> GetFieldsForFileTypeAsync(long fileTypeId)
        {
            List<FileTypeField> fields = await _repository.GetByFileTypeIdAsync(fileTypeId);
            return fields.Select(f => new FileTypeFieldDTO(f)).ToList();
        }

        public async Task<long> CreateAsync(FileTypeFieldDTO fieldDto)
        {
            var field = new FileTypeField(fieldDto);
            return await _repository.CreateAsync(field);
        }

        public async Task<OperationResult> UpdateAsync(FileTypeFieldDTO fieldDto)
        {
            await _repository.UpdateAsync(new FileTypeField(fieldDto));
            return OperationResult.Success();
        }
        public async Task UpdateFieldsForFileTypeAsync(long fileTypeId, List<FileTypeFieldDTO> fields)
        {
            // Удаляем старые поля
            await _repository.DeleteByFileTypeIdAsync(fileTypeId);

            // Добавляем новые
            foreach (var fieldDto in fields)
            {
                fieldDto.FileTypeId = fileTypeId;
                await CreateAsync(fieldDto);
            }
        }
    }
}
