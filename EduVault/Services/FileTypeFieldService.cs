using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using NuGet.Protocol.Core.Types;
using System.Text.RegularExpressions;

namespace EduVault.Services
{
    public interface IFileTypeFieldService
    {
        Task<FileTypeField> GetByIdAsync(long id);
        //Task<FileTypeField> GetByNameAsync(string name);
        Task<OperationResult> DeleteByFileTypeIdAsync(long fileTypeId);
        Task<List<FileTypeFieldDTO>> GetFieldsForFileTypeAsync(long fileTypeId);

        Task<long> CreateAsync(FileTypeFieldDTO fieldDto);
        Task<OperationResult> UpdateAsync(FileTypeFieldDTO fieldDto);
        Task UpdateFieldsForFileTypeAsync(long fileTypeId, List<FileTypeFieldDTO> fields);
        Task<OperationResult> AddFieldToFileTypeAsync(FileTypeField field);
        Task<OperationResult> RemoveFieldFromFileTypeAsync(long fileTypeFieldId);
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
        /*public async Task<FileTypeField> GetByNameAsync(string name)
        {
            return await _repository.GetByNameAsync(name);
        }*/
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
        public async Task<OperationResult> AddFieldToFileTypeAsync(FileTypeField field)
        {
            if((await GetFieldsForFileTypeAsync(field.FileTypeId)).Select(f => f.Name).Contains(field.Name))
            {
                return OperationResult.Failed("У этого типа данных уже существует поле с таким именем!", OperationStatusCode.Conflict);
            }
            await _repository.AddFieldToFileTypeAsync(field);
            return OperationResult.Success();
        }
        public async Task<OperationResult> RemoveFieldFromFileTypeAsync(long fieldId)
        {
            if ((await GetByIdAsync(fieldId)==null))
            {
                return OperationResult.Failed("Поля с таким ID не существует!", OperationStatusCode.Conflict);
            }
            await _repository.RemoveFieldFromFileTypeAsync(fieldId);
            return OperationResult.Success();
        }
    }
}
