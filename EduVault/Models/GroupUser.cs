namespace EduVault.Models
{
    public class GroupUser
    {
        private long _userId;
        private long _groupId;
        public long UserId { get { return _userId; } set { _userId = value; } }
        public User User { get; set; }

        public long GroupId { get { return _groupId; } set { _groupId = value; } }
        public Group Group { get; set; }
        public GroupUser(long userId, long groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
        GroupUser() { }
        ~GroupUser() { }
    }
}
