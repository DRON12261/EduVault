namespace EduVault.Models
{
	public class FileType
	{
		private long _id;
		private string _name;
		private FileTypeField[] _fileTypeFields;

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
		public FileTypeField[] FileTypeFields { get { return _fileTypeFields; } set { _fileTypeFields = value; } }
		FileType()
		{

		}
		~FileType()
		{

		}
	}
}
