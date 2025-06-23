using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class FileTypeFieldDTO
    {
        public long Id { get; set; }

        //[Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; }

        //public string Placeholder { get; set; }
        public bool IsRequired { get; set; }
        public bool IsPrefilled { get; set; }
        [Range(-1, long.MaxValue, ErrorMessage = "Неверный FileTypeId")]
        public long FileTypeId { get; set; }
        //public string DefaultValue { get; set; }
        public FileTypeFieldDTO(FileTypeField fileTypeField)
        {
            Id = fileTypeField.Id;
            Name = fileTypeField.Name;
            IsPrefilled = fileTypeField.IsPrefilled;
            IsRequired = fileTypeField.IsRequired;
            FileTypeId = fileTypeField.FileTypeId;
        }
        public FileTypeFieldDTO() { }

    }
}
