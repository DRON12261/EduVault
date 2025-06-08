namespace EduVault.Models
{
	public class Role : IRecipient
	{
		private long _id;
		private string _name;
		private List<User> _users;
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
		public List<User> Users { get { return _users; } set { _users = value; } }
		Role()
		{

		}
		~Role()
		{

		}
	}
}
