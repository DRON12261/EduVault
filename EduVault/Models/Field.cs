namespace EduVault.Models
{
	public class Field
	{
		private long _id;
		private string _name;
		private string _value;
		private FileTypeField _fileTypeField;

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
		public string Value { get { return _value; } set { _value = value; } }
		public FileTypeField FileTypeField { get { return _fileTypeField; } set { _fileTypeField = value; } }
		Field()
		{

		}
		~Field()
		{

		}
	}
}
