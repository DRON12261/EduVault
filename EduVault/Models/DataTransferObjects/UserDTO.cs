using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class UserDTO
    {
        public long Id {  get; set; }
        public string Name { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public long Role { get; set; }
        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Login = user.Login;
            Role = user.Roleid;
        }
        public UserDTO() {}
    }
}
