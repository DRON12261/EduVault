using EduVault.Models;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduVault.Pages.Groups
{
    [Authorize(Roles = "Администратор")]
    public class IndexModel : PageModel
    {
        private IGroupService _service;
        public List<Group> Groups { get; set; } = new List<Group>();
        public IndexModel(IGroupService service)
        {
            _service = service;
        }
        public async Task OnGetAsync()
        {
            Groups = await _service.GetAllAsync() ?? new List<Group>();
        }
        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            OperationResult result = await _service.DeleteById(id);

            if (!(result.StatusCode == OperationStatusCode.Success))
            {
                TempData["ResultMessage"] = result.Message;
                return RedirectToPage();
            }

            TempData["ResultMessage"] = "Группа успешно удалена";
            return RedirectToPage();
        }
    }
}
