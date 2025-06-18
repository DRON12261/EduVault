using EduVault.Data;
using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace EduVault.Repositories
{
    public interface IFileTypeFieldRepository
    {
        Task<FileTypeField> GetByIdAsync(long id);
        Task<List<FileTypeField>> GetByFileTypeIdAsync(long fileTypeId);
        Task<List<FileTypeField>> GetFieldsForFileTypeAsync(long fileTypeId);

        Task<long> CreateAsync(FileTypeField field);

        Task<OperationResult> UpdateAsync(FileTypeField field);
        Task<OperationResult> DeleteByFileTypeIdAsync(long fileTypeId);
    }
    public class FileTypeFieldRepository: IFileTypeFieldRepository
    {
        private readonly IDbContextFactory<PostgresDBContext> _contextFactory;
        public FileTypeFieldRepository(IDbContextFactory<PostgresDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<FileTypeField> GetByIdAsync(long id)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.FileTypeFields.FindAsync(id);
        }
        public async Task<List<FileTypeField>> GetByFileTypeIdAsync(long fileTypeId)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.FileTypeFields
                .Where(f => f.FileTypeId == fileTypeId)
                .ToListAsync();
        }
        public async Task<List<FileTypeField>> GetFieldsForFileTypeAsync(long fileTypeId)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.FileTypeFields
                .Where(f => f.FileTypeId == fileTypeId)
                .ToListAsync();
        }

        public async Task<long> CreateAsync(FileTypeField field)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            await _context.FileTypeFields.AddAsync(field);
            await _context.SaveChangesAsync();
            return field.Id;
        }

        public async Task<OperationResult> UpdateAsync(FileTypeField field)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            _context.FileTypeFields.Update(field);
            await _context.SaveChangesAsync();
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteByFileTypeIdAsync(long fileTypeId)
        {
            await using var context = _contextFactory.CreateDbContext();
            var fields = await context.FileTypeFields
                .Where(f => f.FileTypeId == fileTypeId)
                .ToListAsync();

            context.FileTypeFields.RemoveRange(fields);
            await context.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
