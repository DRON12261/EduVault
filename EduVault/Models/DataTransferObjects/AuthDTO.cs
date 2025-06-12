using System.ComponentModel.DataAnnotations;

namespace EduVault.Models.DataTransferObjects
{
    public class AuthDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public AuthDTO() { }
    }
}
