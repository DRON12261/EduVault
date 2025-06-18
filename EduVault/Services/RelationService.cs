using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Repositories;
using NuGet.Protocol.Core.Types;

namespace EduVault.Services
{
    public interface IRelationService
    {
        Task<OperationResult> CreateRelationshipAsync(Relation relation);
        Task<OperationResult> DeleteRelationshipAsync(long relationshipId);
        Task<List<Relation>> GetRelationshipsForRecordAsync(long recordId);
    }
    public class RelationService: IRelationService
    {
        private readonly IRelationRepository _repository;
        public RelationService(IRelationRepository repository)
        {
            _repository = repository;
        }
        public async Task<OperationResult> CreateRelationshipAsync(Relation relation)
        {
            if (await _repository.CheckIfExist(relation))
            {
                return OperationResult.Failed("Такая связь уже существует!", OperationStatusCode.Conflict);
            }
            await _repository.CreateAsync(relation);
            return OperationResult.Success();
        }
        public async Task<OperationResult> DeleteRelationshipAsync(long relationshipId)
        {
            if (await _repository.GetByIdAsync(relationshipId) == null)
            {
                return OperationResult.Failed("Связи с таким Id не существует", OperationStatusCode.NotFound);
            }
            await _repository.DeleteAsync(relationshipId);
            return OperationResult.Success();
        }
        public async Task<List<Relation>> GetRelationshipsForRecordAsync(long recordId)
        {
            return await _repository.GetAllForRecordAsync(recordId);
        }
    }
}
