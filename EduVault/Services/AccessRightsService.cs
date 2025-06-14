using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace EduVault.Services
{
    public interface IAccessRightsService
    {
        Task<List<AccessRightsDTO>> GetAccessRightsForRecordAsync(long recordId);
    }
    public class AccessRightsService: IAccessRightsService
    {
        private readonly IAccessRightsRepository _repository;
        public AccessRightsService(IAccessRightsRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<AccessRightsDTO>> GetAccessRightsForRecordAsync(long recordId)
        {
            return await _dbContext.AccessRights
                .Where(ar => ar.RecordId == recordId)
                .Join(_dbContext.Users,
                    ar => ar.UserId,
                    u => u.Id,
                    (ar, u) => new { AccessRight = ar, User = u })
                .Join(_dbContext.AccessRightsTypes,
                    x => x.AccessRight.RightTypeId,
                    art => art.Id,
                    (x, art) => new AccessRightsDTO
                    {
                        UserId = x.User.Id,
                        UserName = x.User.Name,
                        RightTypeId = art.Id,
                        RightTypeName = art.Name
                    })
                .ToListAsync();
        }
    }
}
