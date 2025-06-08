using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EduVault.Services;

namespace EduVault.Pages
{
	[AllowAnonymous]
	public class IndexModel : PageModel
    {
        private IUserService _userService;
        public IndexModel(IUserService userService)
        { 
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _userService.CheckSystemUser();
            return RedirectToPage("Auth/Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}
