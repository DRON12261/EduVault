namespace EduVault.Models
{
	public class AccessRightsType
	{
		private long _id;
		private string _name;
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
		AccessRightsType()
		{

		}
		~AccessRightsType()
		{

		}
	}
}
