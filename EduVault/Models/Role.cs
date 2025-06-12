using EduVault.Models.DataTransferObjects;

namespace EduVault.Models
{
    public class Role
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
        Role() { }
        ~Role() { }
    }
}
