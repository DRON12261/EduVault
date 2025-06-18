using EduVault.Models.DataTransferObjects;

namespace EduVault.Models
{
	public class Relation
	{
		private long _id;
		private long _sourceRecordId;
		private long _targetRecordId;
		public long Id { get { return _id; } set { _id = value; } }
		public long SourceRecord { get { return _sourceRecordId; } set { _sourceRecordId = value; } }
		public long TargetRecord { get { return _targetRecordId; } set { _targetRecordId = value; } }
        public Relation(RelationDTO relationDTO)
        {
            Id = relationDTO.Id;
            SourceRecord = relationDTO.SourceRecordId;
            TargetRecord = relationDTO.TargetRecordId;
        }
        public Relation(long sourceId, long targetId)
        {
            SourceRecord = sourceId;
            TargetRecord = targetId;
        }
        Relation()
		{

		}
		~Relation()
		{

		}
	}
}
