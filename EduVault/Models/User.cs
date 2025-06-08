using EduVault.Services;

namespace EduVault.Models
{
	public class User : IRecipient
	{
		private long _id;
		private string _name;
		private string _login;
		private string _passwordHash;
		private byte _userType;
		private List<Role> _roles;
		public long Id { get { return _id; } }
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
		public List<Role> Roles { get { return _roles; } set { _roles = value; } }
        public User(string login, string password)
		{
            Login = login;
            PasswordHash = password;
		}
        public User(UserCreationDto dto)
        {
            Name = dto.Name;
            Login = dto.Login;
            PasswordHash = dto.Password;
        }
        User() {}
        ~User(){}
	}
}
