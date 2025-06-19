
using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class FilterModel
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public long? FileTypeId { get; set; }
        public string RecordAuthor { get; set; }
        [Display(Name = "От")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [Display(Name = "До")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string Login { get; set; }
        //public byte UserType { get; set; }
        public long? RoleId { get; set; }
    }
}
