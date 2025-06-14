using EduVault.Data;
using EduVault.Models.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace EduVault.Repositories
{
    public interface IAccessRightsRepository
    {
        Task<List<AccessRightsDTO>> GetAccessRightsForRecordAsync(long recordId);
    }
    public class AccessRightsRepository: IAccessRightsRepository
    {
        private readonly IDbContextFactory<PostgresDBContext> _contextFactory;
        public AccessRightsRepository(IDbContextFactory<PostgresDBContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public Task<List<AccessRightsDTO>> GetAccessRightsForRecordAsync(long recordId)
        {

        }
    }
}
