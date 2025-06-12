using EduVault.Services;
using EduVault.Models.DataTransferObjects;

namespace EduVault.Models
{
	public class User : IRecipient
	{
		private long _id;
		private string _name;
		private string _login;
		private string _passwordHash;
		private byte _userType;
        private long _roleid;
		private List<Group> _groups;
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
        public long Roleid { get { return _roleid; } set { _roleid = value; } }
		public List<Group> Groups { get { return _groups; } set { _groups = value; } }
        public User(string name, string login, string password, long roleid)
		{
            Name = name;
            Login = login;
            PasswordHash = password;
            Roleid = roleid;

        }
        public User(UserDTO userDTO)
        {
            Name = userDTO.Name;
            Login = userDTO.Login;
            PasswordHash = userDTO.Password;
            Roleid = userDTO.Role;
        }
        User() {}
        ~User(){}
	}
}
