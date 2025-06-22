using EduVault.Data;
using EduVault.Models;
using Microsoft.EntityFrameworkCore;

namespace EduVault.Repositories
{
    public interface IFieldRepository
    {
        Task SaveFieldsForRecordAsync(long recordId, Dictionary<long, string> fieldValues);
        Task<Dictionary<long, string>> GetFieldsForRecordAsync(long recordId);
    }
    public class FieldRepository: IFieldRepository
    {
        private readonly PostgresDBContext _context;

        public FieldRepository(PostgresDBContext context)
        {
            _context = context;
        }

        public async Task SaveFieldsForRecordAsync(long recordId, Dictionary<long, string> fieldValues)
        {
            // Удаляем старые значения
            var existingFields = await _context.Fields
                .Where(f => f.RecordId == recordId)
                .ToListAsync();

            _context.Fields.RemoveRange(existingFields);

            // Добавляем новые значения
            foreach (var field in fieldValues)
            {
                _context.Fields.Add(new Field
                {
                    RecordId = recordId,
                    FileTypeFieldId = field.Key,
                    Value = field.Value
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<long, string>> GetFieldsForRecordAsync(long recordId)
        {
            return await _context.Fields
                .Where(f => f.RecordId == recordId)
                .ToDictionaryAsync(f => f.FileTypeFieldId, f => f.Value);
        }
    }
}
