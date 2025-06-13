using EduVault.Models.DataTransferObjects;

namespace EduVault.Models
{
	public class Group : IRecipient
	{
		private long _id;
		private string _name;
		private List<User> _users;
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
		public List<User> Users { get { return _users; } set { _users = value; } }
        public Group(GroupDTO groupDTO)
        {
            Id = groupDTO.Id;
            Name = groupDTO.Name;
        }
        Group(){}
		~Group(){}
	}
}
