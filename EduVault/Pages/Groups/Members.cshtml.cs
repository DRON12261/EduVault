using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace EduVault.Pages.Groups
{
    [Authorize(Roles = "Администратор")]
    public class MembersModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public MembersModel(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;
        }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public string GroupName { get; set; }
        public List<UserDTO> GroupMembers { get; set; }
        public List<UserDTO> AvailableUsers { get; set; }

        public async Task OnGetAsync()
        {
            await LoadData();
        }

        public async Task<IActionResult> OnPostAddMember(long userId)
        {
            await _groupService.AddUserToGroupAsync(userId, Id);
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveMember(long userId)
        {
            await _groupService.RemoveUserFromGroupAsync(userId, Id);
            await LoadData();
            return Page();
        }

        private async Task LoadData()
        {
            var group = await _groupService.GetByIdAsync(Id);
            GroupName = group.Name;

            GroupMembers = await _userService.GetUsersForGroupAsync(Id);
            var allUsers = (await _userService.GetAllAsync()).Select(u => new UserDTO(u)).ToList();
            AvailableUsers = allUsers
                .Where(u => !GroupMembers.Any(m => m.Id == u.Id))
                .ToList();
        }
    }
}

