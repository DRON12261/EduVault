using EduVault.Data;
using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System;
using static NuGet.Packaging.PackagingConstants;

namespace EduVault.Repositories
{
    public interface IRecordRepository
    {
        Task<Record> GetByIdAsync(long id);
        Task CreateAsync(Record record);
        Task UpdateAsync(Record record);
        Task DeleteAsync(long id);
        Task<List<Record>> GetAllAsync();
        Task<List<Record>> GetFilteredRecordsAsync(FilterModel filters);


    }
    public class RecordRepository: IRecordRepository
    {
        private readonly IDbContextFactory<PostgresDBContext> _contextFactory;
        public RecordRepository(IDbContextFactory<PostgresDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Record> GetByIdAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Records.FindAsync(id);
        }

        public async Task CreateAsync(Record record)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Record record)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            _context.Records.Update(record);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Record>> GetAllAsync()
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Records
                .AsNoTracking()
                .OrderBy(r=>r.Id)
                .ToListAsync();
        }
        public async Task DeleteAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            var record = await _context.Records.FindAsync(id);
            if (record != null)
            {
                _context.Records.Remove(record);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Record>> GetFilteredRecordsAsync(FilterModel filters)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            var query = _context.Records.AsQueryable();

            if (filters.Id.HasValue)
            {
                query = query.Where(r => r.Id == filters.Id.Value);
            }

            if (!string.IsNullOrEmpty(filters.Name))
            {
                query = query.Where(r => r.Name.Contains(filters.Name));
            }

            if (filters.FileTypeId.HasValue)
            {
                query = query.Where(r => r.FileTypeId == filters.FileTypeId.Value);
            }

            if (!string.IsNullOrEmpty(filters.RecordAuthor))
            {
                query = query.Where(r => _context.Users
                .Where(u => u.Id == r.RecordAuthorId)
                .Any(u => u.Name.Contains(filters.RecordAuthor)));
            }

            if (filters.StartDate.HasValue)
            {
                filters.StartDate = DateTime.SpecifyKind(filters.StartDate.Value, DateTimeKind.Utc);
                query = query.Where(x => x.RecordCreationDate >= filters.StartDate);
            }

            if (filters.EndDate.HasValue)
            {
                // Добавляем 1 день, чтобы включить все записи за указанный день
                var dateToInclusive = DateTime.SpecifyKind(filters.EndDate.Value, DateTimeKind.Utc).AddDays(1);
                query = query.Where(x => x.RecordCreationDate < dateToInclusive);
            }
            return await query
                .OrderBy(r => r.Id)
                .Select(r => r)
                .ToListAsync();
        }
    }
}
