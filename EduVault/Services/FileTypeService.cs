using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using NuGet.Protocol.Core.Types;

namespace EduVault.Services
{
    public interface IFileTypeService
    {
        Task<List<FileType>> GetAllAsync();
        Task<FileType> GetByIdAsync(long id);
        Task<OperationResult> CreateAsync(FileTypeDTO fileTypeDTO);
        Task<OperationResult> UpdateAsync(FileTypeDTO fileTypeDTO);
        Task<OperationResult> DeleteById(long id);
    }
    public class FileTypeService: IFileTypeService
    {
        private readonly IFileTypeRepository _fileTypeRepository;
        private readonly IFileTypeFieldService _fileTypeFieldService;
        public FileTypeService(IFileTypeRepository fileTypeRepository, IFileTypeFieldService fileTypeFieldService)
        {
            _fileTypeRepository = fileTypeRepository;
            _fileTypeFieldService = fileTypeFieldService;
        }
        public async Task<List<FileType>> GetAllAsync()
        {
            return await _fileTypeRepository.GetAllAsync();
        }
        public async Task<FileType> GetByIdAsync(long id)
        {
            FileType ft = await _fileTypeRepository.GetByIdAsync(id);
            if (ft!=null) ft.FileTypeFields = (await _fileTypeFieldService.GetFieldsForFileTypeAsync(id)).Select(f=>new FileTypeField(f)).ToList();
            return ft;
        }
        public async Task<OperationResult> CreateAsync(FileTypeDTO fileTypeDTO)
        {
            if (await _fileTypeRepository.GetByNameAsync(fileTypeDTO.Name) != null)
                return OperationResult.Failed("Такой тип файлов уже существует!", OperationStatusCode.Conflict);

            await _fileTypeRepository.CreateAsync(new FileType(fileTypeDTO));
            return OperationResult.Success();
        }
        public async Task<OperationResult> UpdateAsync(FileTypeDTO fileTypeDTO)
        {
            if (await _fileTypeRepository.GetByIdAsync(fileTypeDTO.Id) == null)
                return OperationResult.Failed("Типа файлов с таким Id не существует!", OperationStatusCode.NotFound);

            await _fileTypeRepository.UpdateAsync(new FileType(fileTypeDTO));

            // Полностью заменяем коллекцию полей
            await _fileTypeFieldService.UpdateFieldsForFileTypeAsync(
                fileTypeDTO.Id,
                fileTypeDTO.FileTypeFields);
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteById(long id)
        {
            if (await _fileTypeRepository.GetByIdAsync(id) == null)
            {
                return OperationResult.Failed("Типа файлов с таким Id не существует!", OperationStatusCode.NotFound);
            }
            await _fileTypeRepository.DeleteAsync(id);
            return OperationResult.Success();
        }
    }
}
