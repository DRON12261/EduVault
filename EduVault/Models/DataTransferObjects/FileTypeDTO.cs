using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class FileTypeDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        public FileTypeDTO(FileType fileType)
        {
            Id = fileType.Id;
            Name = fileType.Name;
        }
        public FileTypeDTO() {}
    }
}
