namespace EduVault.Models
{
	public class Field
	{
		private long _id;
		private string _name;
		private string _value;
		private long _fileTypeFieldId;
        private long _recordId;

		public long Id { get { return _id; } set { _id = value; } }
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				try
				{
					_name = value;
				}
				catch
				{
					throw new ArgumentException("Попытка записать нестроку");

				}
			}
		}
		public string Value { get { return _value; } set { _value = value; } }
		public long FileTypeFieldId { get { return _fileTypeFieldId; } set { _fileTypeFieldId = value; } }
        public long RecordId { get { return _recordId; } set { _recordId = value; } }
        // Навигационные свойства
        public Record Record { get; set; }
        public FileTypeField FileTypeField { get; set; }
        public Field()
		{

		}
		~Field()
		{

		}
	}
}
