using EduVault.Data;
using EduVault.Models;
using EduVault.Repositories;

namespace EduVault.Services
{
    public interface IFieldService
    {
        Task SaveFieldsForRecordAsync(long recordId, Dictionary<long, string> fieldValues);
        Task<Dictionary<long, string>> GetFieldsForRecordAsync(long recordId);
    }

    public class FieldService : IFieldService
    {
        private readonly IFieldRepository _repository;

        public FieldService(IFieldRepository repository)
        {
            _repository = repository;
        }

        public async Task SaveFieldsForRecordAsync(long recordId, Dictionary<long, string> fieldValues)
        {
            await _repository.SaveFieldsForRecordAsync(recordId, fieldValues);
        }

        public async Task<Dictionary<long, string>> GetFieldsForRecordAsync(long recordId)
        {
            return await _repository.GetFieldsForRecordAsync(recordId);
        }
    }
}
