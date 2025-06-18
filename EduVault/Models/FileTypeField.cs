using EduVault.Models.DataTransferObjects;

namespace EduVault.Models
{
	public class FileTypeField
	{
		private long _id;
		private string _name;
		private bool _isRequired;
		private bool _isPrefilled;
        private long _fileTypeId;
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
		public bool IsRequired { get { return _isRequired; } set { _isRequired = value; } }
		public bool IsPrefilled { get { return _isPrefilled; } set { _isPrefilled = value; } }
        public long FileTypeId { get { return _fileTypeId; } set { _fileTypeId = value; } }
        public FileTypeField(FileTypeFieldDTO fileTypeFieldDTO)
        {
            Id = fileTypeFieldDTO.Id;
            Name = fileTypeFieldDTO.Name;
            //Placeholder = fieldDto.Placeholder;
            IsRequired = fileTypeFieldDTO.IsRequired;
            IsPrefilled = fileTypeFieldDTO.IsPrefilled;
            FileTypeId = fileTypeFieldDTO.FileTypeId;
        }
        FileTypeField()
		{

		}
		~FileTypeField()
		{

		}
	}
}
