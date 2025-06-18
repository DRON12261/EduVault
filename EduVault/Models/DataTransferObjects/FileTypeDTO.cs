using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class FileTypeDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FileNameTemplate { get; set; }
        public List<FileTypeFieldDTO> FileTypeFields { get; set; } = new();

        public FileTypeDTO(FileType fileType)
        {
            Id = fileType.Id;
            Name = fileType.Name;
            FileNameTemplate = fileType.FileNameTemplate;
            FileTypeFields = fileType.FileTypeFields.Select(f=>new FileTypeFieldDTO(f)).ToList();
        }
        public FileTypeDTO() {}
    }
}
