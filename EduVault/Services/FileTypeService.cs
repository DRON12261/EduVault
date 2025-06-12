using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;

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
        public FileTypeService(IFileTypeRepository fileTypeRepository)
        {
            _fileTypeRepository = fileTypeRepository;
        }
        public async Task<List<FileType>> GetAllAsync()
        {
            return await _fileTypeRepository.GetAllAsync();
        }
        public async Task<FileType> GetByIdAsync(long id)
        {
            return await _fileTypeRepository.GetByIdAsync(id);
        }
        public async Task<OperationResult> CreateAsync(FileTypeDTO fileTypeDTO)
        {
            if (await _fileTypeRepository.GetByNameAsync(fileTypeDTO.Name) != null)
                return OperationResult.Failed("Такой тип файлов уже существует!", OperationStatusCode.Conflict);

            await _fileTypeRepository.AddAsync(new FileType(fileTypeDTO));
            return OperationResult.Success();
        }
        public async Task<OperationResult> UpdateAsync(FileTypeDTO fileTypeDTO)
        {
            if (await _fileTypeRepository.GetByIdAsync(fileTypeDTO.Id) == null)
                return OperationResult.Failed("Типа файлов с таким Id не существует!", OperationStatusCode.NotFound);

            await _fileTypeRepository.UpdateAsync(new FileType(fileTypeDTO));
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
