namespace EduVault.DBClasses
{
	public class Relation
	{
		private long _id;
		private Record _sourceRecord;
		private Record _targetRecord;
		public long Id { get { return _id; } }
		public Record SourceRecord { get { return _sourceRecord; } set { _sourceRecord = value; } }
		public Record TargetRecord { get { return _targetRecord; } set { _targetRecord = value; } }
		Relation()
		{

		}
		~Relation()
		{

		}
	}
}
