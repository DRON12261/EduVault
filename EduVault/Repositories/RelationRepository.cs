using EduVault.Data;
using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
//using MongoDB.Driver.Linq;

namespace EduVault.Repositories
{
    public interface IRelationRepository
    {
        Task<Relation> GetByIdAsync(long id);
        Task CreateAsync(Relation relation);
        Task<bool> CheckIfExist(Relation relation);
        //Task UpdateAsync(Relation relation);
        Task DeleteAsync(long id);
        Task<List<Relation>> GetAllAsync();
        Task<List<Relation>> GetAllForRecordAsync(long id);
    }
    public class RelationRepository: IRelationRepository
    {
        private readonly IDbContextFactory<PostgresDBContext> _contextFactory;
        public RelationRepository(IDbContextFactory<PostgresDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Relation> GetByIdAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Relationships.FindAsync(id);
        }
        public async Task CreateAsync(Relation relation)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            await _context.Relationships.AddAsync(relation);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckIfExist(Relation relation)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Relationships
                .AnyAsync(r =>
                    r.SourceRecord == relation.SourceRecord &&
                    r.TargetRecord == relation.TargetRecord);
        }
        public async Task DeleteAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
        }
        public async Task<List<Relation>> GetAllAsync()
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Relationships
               .AsNoTracking()
               .OrderBy(r => r.Id)
               .ToListAsync();
        }
        public async Task<List<Relation>> GetAllForRecordAsync(long recordId)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Relationships
                    .Where(rel => rel.SourceRecord == recordId)
                    .ToListAsync();
        }
    }
}
