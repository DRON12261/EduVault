using EduVault.Data;
using EduVault.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace EduVault.Repositories
{
    public interface IFileTypeRepository
    {
        Task<FileType> GetByIdAsync(long id);
        Task<FileType> GetByNameAsync(string name);
        Task CreateAsync(FileType fileType);
        Task UpdateAsync(FileType fileType);
        Task DeleteAsync(long id);
        Task<List<FileType>> GetAllAsync();
    }
    public class FileTypeRepository: IFileTypeRepository
    {
        private readonly IDbContextFactory<PostgresDBContext> _contextFactory;
        public FileTypeRepository(IDbContextFactory<PostgresDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<FileType> GetByIdAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.FileTypes.FindAsync(id);
        }
        public async Task<FileType> GetByNameAsync(string name)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.FileTypes.FirstOrDefaultAsync(fileType => fileType.Name == name);
        }
        public async Task CreateAsync(FileType filetype)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            await _context.FileTypes.AddAsync(filetype);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FileType filetype)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            _context.FileTypes.Update(filetype);
            await _context.SaveChangesAsync();
        }
        public async Task<List<FileType>> GetAllAsync()
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            return await _context.FileTypes
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task DeleteAsync(long id)
        {
            await using PostgresDBContext _context = _contextFactory.CreateDbContext();
            var filetype = await _context.FileTypes.FindAsync(id);
            if (filetype != null)
            {
                _context.FileTypes.Remove(filetype);
                await _context.SaveChangesAsync();
            }
        }
    }
}
