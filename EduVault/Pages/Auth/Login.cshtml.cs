using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations;

namespace EduVault.Pages.Auth
{
    [AllowAnonymous]
    
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        public LoginModel(IAuthService authService)
        {
            _authService = authService;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Login { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
                return Page();

            // Проверка логина и пароля
            if (!await _authService.ValidateUserAsync(Input.Login, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Неверные данные");
                return RedirectToPage("/Auth/AccessDenied");
            }

            // Создание куки
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Input.Login),
            new Claim(ClaimTypes.Role, "User") // Пример роли
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            // Перенаправление на целевую страницу
            return RedirectToPage("/Table");
        }
    }
}
