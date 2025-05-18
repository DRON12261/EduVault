using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduVault.Pages
{
	[AllowAnonymous]
	public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("Login");
        }
    }
}
