using EduVault.Services;
using EduVault.Models.DataTransferObjects;
using bCrypt = BCrypt.Net.BCrypt;

namespace EduVault.Models
{
	public class User : IRecipient
	{
		private long _id;
		private string _name;
		private string _login;
		private string _passwordHash;
		private byte _userType;
        private int _roleId;
		private List<GroupUser> _groups;
		public long Id { get { return _id; } set { _id = value; } }
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				try
				{
					_name = value;
				}
				catch
				{
					throw new ArgumentException("Попытка записать нестроку");

				}
			}
		}
		public string Login { get { return _login; } set { _login = value; } }
		public string PasswordHash { get { return _passwordHash; } set { _passwordHash = value; } }
		public byte UserType { get { return _userType; } set { _userType = value; } }
        public int RoleId { get { return _roleId; } set { _roleId = value; } }
        public List<GroupUser> Groups { get { return _groups; } set { _groups = value; } }
        public User(string name, string login, string password, int roleId)
		{
            Name = name;
            Login = login;
            PasswordHash = password;
            RoleId = roleId;
        }
        public User(UserDTO userDTO)
        {
            Id = userDTO.Id;
            Name = userDTO.Name;
            Login = userDTO.Login;
            PasswordHash = bCrypt.HashPassword(userDTO.Password);
            RoleId = userDTO.Role;
        }
        User() {}
        ~User(){}
	}
}
