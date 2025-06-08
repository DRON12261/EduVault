using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduVault.Pages.Auth
{
    [AllowAnonymous] // Разрешаем доступ без авторизации
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {
            // Логика для GET-запроса
            // Можно добавить дополнительную информацию в ViewData
            ViewData["OriginalPath"] = HttpContext.Request.Query["ReturnUrl"];
        }
    }
}
