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
        private IGroupService _service;
        public GroupModel(IGroupService service)
        {
            _service = service;
        }
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }
        [BindProperty]
        public GroupDTO Input { get; set; }
        public IActionResult OnGet()
        {
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Mode == "create")
            {
                Input = new GroupDTO();
            }
            else if (Mode == "edit")
            {
                Input = new GroupDTO(await _service.GetByIdAsync(Id));
            }
            return Page();
        }
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            if (Mode == "create")
            {
                await _service.CreateAsync(Input);
            }
            else if (Mode == "edit")
            {
                await _service.UpdateAsync(Input);
            }
            return RedirectToPage("/Groups/Index");
        }
    }
}
