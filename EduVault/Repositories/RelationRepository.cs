using EduVault.Data;
using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
//using MongoDB.Driver.Linq;

namespace EduVault.Repositories
{
    public interface IRelationRepository
    {
        Task<Relation> GetByIdAsync(long id);
        Task CreateAsync(Relation relation);
        Task<bool> CheckIfExist(Relation relation);
        //Task UpdateAsync(Relation relation);
        Task DeleteAsync(Relation relation);
        Task<List<Relation>> GetAllAsync();
        Task<List<Record>> GetRelatedRecordsForRecordAsync(long id);
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
                    r.SourceRecordId == relation.SourceRecordId &&
                    r.TargetRecordId == relation.TargetRecordId);
        }
        public async Task DeleteAsync(Relation relation)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            if(await CheckIfExist(relation))
            {
                _context.Remove(relation);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Relation>> GetAllAsync()
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Relationships
               .AsNoTracking()
               .OrderBy(r => r.SourceRecordId)
               .ToListAsync();
        }
        public async Task<List<Record>> GetRelatedRecordsForRecordAsync(long recordId)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            List<Record> src = await _context.Relationships
                .Join(
                   _context.Records,
                   relation => relation.SourceRecordId,
                   record => record.Id,
                   (relation, record) => new { Relationships = relation, Records = record }
                )
                .Where(x => x.Relationships.TargetRecordId == recordId)
                .Select(x => x.Records)
                .ToListAsync();
            List<Record> trgt = await _context.Relationships
                .Join(
                   _context.Records,
                   relation => relation.TargetRecordId,
                   record => record.Id,
                   (relation, record) => new { Relationships = relation, Records = record }
                )
                .Where(x => x.Relationships.SourceRecordId == recordId)
                .Select(x => x.Records)
                .ToListAsync();
            return src.Union(trgt).ToList();
        }
        public async Task<List<Relation>> GetAllForRecordAsync(long recordId)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.Relationships
                    .Where(rel => rel.SourceRecordId == recordId || rel.TargetRecordId == recordId)
                    .ToListAsync();
        }
    }
}
