namespace EduVault.Models
{
	public class Record
	{
		private long _id;
		private string _name;
		private string _filePath;
		private User _author;
		private FileType _fileType;
		private Field[] _fields;

		public long Id { get { return _id; } }
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
		public string FilePath { get { return _filePath; } set { _filePath = value; } }
		public User Author { get { return _author; } set { _author = value; } }
		public FileType FileType { get { return _fileType; } set { _fileType = value; } }
		public Field[] Fields { get { return _fields; } set { _fields = value; } }
		Record()
		{

		}
		~Record()
		{

		}

		public AccessRights[] GetAccessRights()
		{
			return Array.Empty<AccessRights>();
		}
		public Relation[] GetRelations()
		{
			return Array.Empty<Relation>();
		}
	}
}
