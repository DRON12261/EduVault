using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EduVault.Models;
using EduVault.Data;
using EduVault.Services;
using Npgsql.TypeMapping;

namespace EduVault.Pages.Users
{
	[Authorize(Roles="Администратор")]
	public class IndexModel : PageModel
	{
        private IRoleService _roleService;
        private IUserService _userService;
        public List<User> Users { get; set; } = new List<User>();
        public List<Role> Roles { get; set; } = new List<Role>();
        public IndexModel(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            Roles = await _roleService.GetAllAsync() ?? new List<Role>();
            Users =  await _userService.GetAllAsync() ?? new List<User>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            OperationResult result = await _userService.DeleteById(id);

            if (!(result.StatusCode == OperationStatusCode.Success))
            {
                TempData["ResultMessage"] = result.Message;
                return RedirectToPage();
            }

            TempData["ResultMessage"] = "Пользователь успешно удалён";
            return RedirectToPage();
        }
        
	}
}
