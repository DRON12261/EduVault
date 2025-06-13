using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class RecordDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string FilePath { get; set; }
        public long RecordAuthor { get; set; }
        public long FileType { get; set; }
        public DateTime RecordCreationDate { get; set; }
        public Field[] Fields;
        public RecordDTO(Record record)
        {
            Id = record.Id;
            Name = record.Name;
            FilePath = record.FilePath;
            RecordAuthor = record.RecordAuthorId;
            FileType = record.FileTypeId;
            RecordCreationDate = record.RecordCreationDate;
        }
        public RecordDTO() { }
    }
}
