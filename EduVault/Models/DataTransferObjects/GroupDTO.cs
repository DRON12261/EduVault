using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class GroupDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }

        public GroupDTO(Group group)
        {
            Id = group.Id;
            Name = group.Name;
        }
        public GroupDTO() { }
    }
}
