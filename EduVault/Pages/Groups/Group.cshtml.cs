using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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

        public List<UserDTO> GroupMembers { get; set; } = new();
        public List<UserDTO> AvailableUsers { get; set; } = new();
        public async Task<IActionResult> OnGetAsync()
        {
            if (Mode == "create")
            {
                Input = new GroupDTO();
                GroupMembers = new List<UserDTO>();
            }
            else if (Mode == "edit")
            {
                if (Id == 0 && TempData["SavedId"] is string savedIdStr && long.TryParse(savedIdStr, out var savedId))
                {
                    Id = savedId;
                    TempData.Keep("SavedId");
                }
                TempData["SavedId"] = Id.ToString();
                Input = new GroupDTO(await _groupService.GetByIdAsync(Id));
                if (Input == null)
                    return NotFound();
                GroupMembers = (await _userService.GetUsersForGroupAsync(Id)).ToList();
                //TempData["TempMembers"] = JsonSerializer.Serialize(GroupMembers);
            }
            var allUsers = (await _userService.GetAllAsync()).Select(u => new UserDTO(u)).ToList();
            AvailableUsers = allUsers
                .Where(u => !GroupMembers.Any(m => m.Id == u.Id))
                .ToList();
            return Page();
        }

        
        public async Task<IActionResult> OnPostSaveAsync()
        {
            await LoadGroupData();
            Input.Users = GroupMembers;
            ModelState.Remove("Input.Users");
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
        private async Task LoadGroupData()
        {
            if (TempData["SavedId"] is string savedIdStr && long.TryParse(savedIdStr, out var savedId))
            {
                Id = savedId;
                TempData.Keep("SavedId");
            }
            if (Mode == "create")
            {
                Input = new GroupDTO();
                GroupMembers = new List<UserDTO>();
            }
            else if (Mode == "edit")
            {
                Input = new GroupDTO(await _groupService.GetByIdAsync(Id));
                GroupMembers = await _userService.GetUsersForGroupAsync(Id);
            }

            // Получаем всех пользователей и исключаем уже добавленных
            var allUsers = (await _userService.GetAllAsync()).Select(u => new UserDTO(u)).ToList();
            AvailableUsers = allUsers
                .Where(u => !GroupMembers.Any(m => m.Id == u.Id))
                .ToList();
        }
        public async Task<IActionResult> OnGetAvailableUsers()
        {
            await LoadGroupData();
            return Partial("_AvailableUsersDropdown", this);
        }
    }
}
