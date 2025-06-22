using EduVault.Models.DataTransferObjects;

namespace EduVault.Repositories
{
    public interface IGroupUserRepository
    {
        Task CreateAsync(long groupId, List<UserDTO> users);
        Task UpdateUsersForGroupAsync(long groupId, List<UserDTO> users);
    }
    public class GroupUserRepository: IGroupUserRepository
    {
        public async Task CreateAsync(long groupId, List<UserDTO> users)
        {

        }
        public async Task UpdateUsersForGroupAsync(long groupId, List<UserDTO> users)
        {

        }
    }
}
