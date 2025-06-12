using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EduVault.Models;
using EduVault.Data;
using EduVault.Services;
using Npgsql.TypeMapping;

namespace EduVault.Pages.Users
{
	//[Authorize]
	public class IndexModel : PageModel
	{
        private IUserService _userService;
        public List<User> Users { get; set; } = new List<User>();
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            Users =  await _userService.GetAllAsync() ?? new List<User>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            OperationResult result = await _userService.DeleteById(id);

            if (!(result.StatusCode == OperationStatusCode.Success))
            {
                TempData["ResultMessage"] = result.ErrorMessage;
                return RedirectToPage();
            }

            TempData["ResultMessage"] = "Пользователь успешно удалён";
            return RedirectToPage();
        }
        
	}
}
