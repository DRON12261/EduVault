using EduVault.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using EduVault.Models.DataTransferObjects;

namespace EduVault.Models
{
	public class Record
	{
		private long _id;
		private string _name;
		private string _filePath;
		private long _recordAuthorId;
		private long _fileTypeId;
        private DateTime _recordCreationDate;
		private Field[] _fields;

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
		public string FilePath { get { return _filePath; } set { _filePath = value; } }
		public long RecordAuthorId { get { return _recordAuthorId; } set { _recordAuthorId = value; } }
		public long FileTypeId { get { return _fileTypeId; } set { _fileTypeId = value; } }
        public DateTime RecordCreationDate { get { return _recordCreationDate; } set { _recordCreationDate = value; } }
        public Field[] Fields { get { return _fields; } set { _fields = value; } }
        public Record(RecordDTO dto)
        {
            Id = dto.Id;
            Name = dto.Name;
            FilePath = dto.FilePath;
            RecordAuthorId = dto.RecordAuthor;
            FileTypeId = dto.FileType;
            RecordCreationDate = dto.RecordCreationDate;
        }
        Record(){}
		~Record(){}

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
