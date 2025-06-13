using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class UserDTO
    {
        public long Id {  get; set; }
        public string Name { get; set; }
        [Required]
        public string Login { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public int Role { get; set; }
        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Login = user.Login;
            Password = user.PasswordHash;
            Role = user.RoleId;
        }
        public UserDTO() {}
    }
}
