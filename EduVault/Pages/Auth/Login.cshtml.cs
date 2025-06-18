using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations;
using EduVault.Services;
using EduVault.Models.DataTransferObjects;

namespace EduVault.Pages.Auth
{
    [AllowAnonymous]
    
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public LoginModel(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [BindProperty]
        public AuthDTO Input { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Input = new AuthDTO();
            await _userService.CheckSystemUser();
            // Проверяем авторизацию через наличие кастомных кук
            //return HttpContext.Request.Cookies.ContainsKey("EduVault.AuthCookie")? RedirectToPage("/Records/Index") : Page();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            // Проверка логина и пароля
            var role = await _authService.ValidateUserAsync(Input);
            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Неверные данные");
                return RedirectToPage("/Auth/AccessDenied");
            }

            // Создание куки
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Input.Login),
                new Claim(ClaimTypes.Role, role.Name)
            };

            
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            
            // Перенаправление на целевую страницу
            return RedirectToPage("/Records/Index");
        }
    }
}
