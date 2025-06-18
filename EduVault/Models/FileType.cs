using EduVault.Models.DataTransferObjects;
using System.Text.RegularExpressions;

namespace EduVault.Models
{
	public class FileType
	{
		private long _id;
		private string _name;
		private List<FileTypeField> _fileTypeFields;
        private string _fileNameTemplate;
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
		public List<FileTypeField> FileTypeFields { get { return _fileTypeFields; } set { _fileTypeFields = value; } }
        public string FileNameTemplate {
            get { return _fileNameTemplate; }
            set
            {
                if (!IsValidTemplate(value))
                    throw new ArgumentException("Некорректный шаблон имени файла");
                _fileNameTemplate = value;
            }
        }
        public FileType(FileTypeDTO fileTypeDTO)
        {
            Id = fileTypeDTO.Id;
            Name = fileTypeDTO.Name;
            FileNameTemplate = fileTypeDTO.FileNameTemplate;
            FileTypeFields = fileTypeDTO.FileTypeFields.Select(f=>new FileTypeField(f)).ToList();
        }
        private static bool IsValidTemplate(string template)
        {
            // Простая валидация шаблона
            return !string.IsNullOrWhiteSpace(template) &&
                   !ContainsAnyInvalidChars(template);
        }
        public static bool ContainsAnyInvalidChars(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            // Регулярное выражение для проверки:
            // - Запрещённые символы в именах файлов
            // - Последовательности из двух точек
            // - Пробелы в начале/конце
            var invalidCharsRegex = new Regex(
                @"[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + @"]|" +
                @"\.{2}|" +
                @"^\s|\s$");

            return invalidCharsRegex.IsMatch(input);
        }
        FileType(){}
		~FileType(){}
	}
}
