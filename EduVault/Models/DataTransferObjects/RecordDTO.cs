using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string FileName { get; set; } // Дополнительное поле для отображения
        // Для шаблонов полей (из FileTypeFields)
        public Dictionary<long, string> CustomFieldsTemplate { get; set; } = new();
        // Для значений полей (будет сохраняться в Fields)
        public Dictionary<long, string> CustomFieldsValues { get; set; } = new();
        public RecordDTO(Record record)
        {
            Id = record.Id;
            Name = record.Name;
            FilePath = record.FilePath;
            RecordAuthor = record.RecordAuthorId;
            FileType = record.FileTypeId;
            //RecordCreationDate = DateTime.SpecifyKind(record.RecordCreationDate.ToLocalTime(), DateTimeKind.Utc);
            RecordCreationDate = record.RecordCreationDate.ToLocalTime();
        }
        public RecordDTO() { }
    }
}
