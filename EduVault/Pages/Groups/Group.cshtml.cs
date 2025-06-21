using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduVault.Pages.Groups
{
    [Authorize(Roles = "Администратор")]
    public class GroupModel : PageModel
    {
        private IGroupService _groupService;
        private IUserService _userService;
        public GroupModel(IGroupService groupService, IUserService userService)
        {
            _groupService = groupService;
            _userService = userService;
        }
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }
        [BindProperty]
        public GroupDTO Input { get; set; }
        public class GroupMemberViewModel
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Login { get; set; }
            public GroupMemberViewModel(UserDTO userDTO)
            {
                this.Id = userDTO.Id;
                this.Name = userDTO.Name;
                this.Login = userDTO.Login;
            }
        }

        public List<UserDTO> GroupMembers { get; set; } = new();
        public List<UserDTO> AvailableUsers { get; set; } = new();
        public IActionResult OnGet()
        {
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Mode == "create")
            {
                Input = new GroupDTO();
                GroupMembers = (await _userService.GetUsersForGroupAsync(Id)).ToList();
                AvailableUsers = (await _userService.GetAllAsync()).Select(u => new UserDTO(u)).ToList();
            }
            else if (Mode == "edit")
            {
                TempData["SavedId"] = Id.ToString();
                Input = new GroupDTO(await _groupService.GetByIdAsync(Id));
                if (Input == null)
                    return NotFound();
                GroupMembers = (await _userService.GetUsersForGroupAsync(Id)).ToList();
                AvailableUsers = (await _userService.GetAllAsync()).Select(u => new UserDTO(u)).ToList();
                //TempData["TempFields"] = JsonSerializer.Serialize(GroupMembers);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            if (Mode == "create")
            {
                await _groupService.CreateAsync(Input);
            }
            else if (Mode == "edit")
            {
                await _groupService.UpdateAsync(Input);
            }
            return RedirectToPage("/Groups/Index");
        }
    }
}
