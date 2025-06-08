using EduVault.Data;
using EduVault.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EduVault.Repositories
{
    public interface IRecordRepository
    {
        Task<Record> GetByIdAsync(long id);
        Task AddAsync(Record record);
        Task UpdateAsync(Record record);
        Task DeleteAsync(long id);
        Task<List<Record>> GetAllAsync();
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

        public async Task AddAsync(Record user)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            await _context.Records.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Record user)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            await _context.Records.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Record>> GetAllAsync()
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Records
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task DeleteAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
