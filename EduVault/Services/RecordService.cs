using BCrypt.Net;
using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using Microsoft.CodeAnalysis.Scripting;

namespace EduVault.Services
{
    public interface IRecordService
    {
        Task<List<Record>> GetAllAsync();
        Task<Record> GetByIdAsync(long id);
        Task<OperationResult> CreateAsync(RecordDTO dto);
        Task<OperationResult> UpdateAsync(RecordDTO dto);
        Task<OperationResult> DeleteById(long id);
    }
    public class RecordService: IRecordService
    {
        private readonly IRecordRepository _repository;
        public RecordService(IRecordRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Record>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<Record> GetByIdAsync(long id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<OperationResult> CreateAsync(RecordDTO dto)
        {
            // Валидация - хз пока как валидировать


            // Создание entity
            await _repository.CreateAsync(new Record(dto));
            return OperationResult.Success();
        }
        public async Task<OperationResult> UpdateAsync(RecordDTO dto)
        {
            await _repository.UpdateAsync(new Record(dto));
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteById(long id)
        {
            if (await _repository.GetByIdAsync(id) == null)
            {
                return OperationResult.Failed("Записи с таким Id не существует", OperationStatusCode.NotFound);
            }
            await _repository.DeleteAsync(id);
            return OperationResult.Success();
        }
    }
}
