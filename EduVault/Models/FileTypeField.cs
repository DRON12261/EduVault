namespace EduVault.DBClasses
{
	public class FileTypeField
	{
		private long _id;
		private string _name;
		private bool _isRequired;
		private bool _isPrefilled;
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
		public bool IsRequired { get { return _isRequired; } set { _isRequired = value; } }
		public bool IsPrefilled { get { return _isPrefilled; } set { _isPrefilled = value; } }
		FileTypeField()
		{

		}
		~FileTypeField()
		{

		}
	}
}
