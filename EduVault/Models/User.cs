namespace EduVault.DBClasses
{
	public class User : IRecipient
	{
		private long _id;
		private string _name;
		private string _login;
		private string _password;
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
		public string Password { get { return _password; } set { _password = value; } }
		public byte UserType { get { return _userType; } set { _userType = value; } }
		public List<Role> Roles { get { return _roles; } set { _roles = value; } }
		User()
		{

		}
		~User()
		{

		}
	}
}
