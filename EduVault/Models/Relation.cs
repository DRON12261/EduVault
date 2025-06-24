using EduVault.Models.DataTransferObjects;

namespace EduVault.Models
{
	public class Relation
	{
		private long _sourceRecordId;
		private long _targetRecordId;
		public long SourceRecordId { get { return _sourceRecordId; } set { _sourceRecordId = value; } }
        public Record SourceRecord { get; set; }
        public long TargetRecordId { get { return _targetRecordId; } set { _targetRecordId = value; } }
        public Record TargetRecord { get; set; }
        public Relation(RelationDTO relationDTO)
        {
            SourceRecordId = relationDTO.SourceRecordId;
            TargetRecordId = relationDTO.TargetRecordId;
        }
        public Relation(long sourceId, long targetId)
        {
            SourceRecordId = sourceId;
            TargetRecordId = targetId;
        }
        Relation()
		{

		}
		~Relation()
		{

		}
	}
}
