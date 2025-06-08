using EduVault.Models;
using EduVault.Repositories;

namespace EduVault.Services
{
    public interface IFileTypeService
    {
        Task<List<FileType>> GetAllFileTypesAsync();
        Task<OperationResult> DeleteFileTypeById(long id);
        Task<OperationResult> CreateFileTypeAsync(string name);
    }
    public class FileTypeService: IFileTypeService
    {
        private readonly IFileTypeRepository _fileTypeRepository;
        public FileTypeService(IFileTypeRepository fileTypeRepository)
        {
            _fileTypeRepository = fileTypeRepository;
        }
        public async Task<List<FileType>> GetAllFileTypesAsync()
        {
            return await _fileTypeRepository.GetAllAsync();
        }
        public async Task<OperationResult> DeleteFileTypeById(long id)
        {
            if (await _fileTypeRepository.GetByIdAsync(id) == null)
            {
                return OperationResult.Failed("Пользователя с таким Id не существует!", OperationStatusCode.NotFound);
            }
            await _fileTypeRepository.DeleteAsync(id);
            return OperationResult.Success();
        }
        public async Task<OperationResult> CreateFileTypeAsync(string name)
        {
            // Валидация
            if (await _fileTypeRepository.GetByNameAsync(name) != null)
                return OperationResult.Failed("Такой тип файлов уже существует!", OperationStatusCode.Conflict);

            // Создание entity
            var fileType = new FileType(name);

            await _fileTypeRepository.AddAsync(fileType);
            return OperationResult.Success();
        }
    }
}
