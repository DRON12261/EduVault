using BCrypt.Net;
using EduVault.Models;
using EduVault.Repositories;

namespace EduVault.Services
{
    public interface IRecordService
    {
        Task<List<Record>> GetAllRecordsAsync();
        Task<OperationResult> CreateRecordAsync(RecordCreationDto dto);
        Task<OperationResult> DeleteRecordById(long id);
    }
    public class RecordService: IRecordService
    {
        private readonly IRecordRepository _recordRepository;
        public RecordService(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }
        public async Task<List<Record>> GetAllRecordsAsync()
        {
            return await _recordRepository.GetAllAsync();
        }
        public async Task<OperationResult> CreateRecordAsync(RecordCreationDto dto)
        {
            // Валидация - хз пока как валидировать
            

            // Создание entity
            var record = new Record(dto);

            await _recordRepository.AddAsync(record);
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteRecordById(long id)
        {
            if (await _recordRepository.GetByIdAsync(id) == null)
            {
                return OperationResult.Failed("Записи с таким Id не существует", OperationStatusCode.NotFound);
            }
            await _recordRepository.DeleteAsync(id);
            return OperationResult.Success();
        }
    }
    public class RecordCreationDto
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public long Author { get; set; }
        public FileType FileType { get; set; }
        public Field[] Fields;
        public RecordCreationDto(string name, string filePath, long author, FileType fileType)
        {
            Name = name;
            FilePath = filePath;
            Author = author;
            FileType = fileType;
        }
    }
}
