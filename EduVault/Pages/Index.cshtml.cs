using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduVault.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Login { get; set; } = "";
        [BindProperty]
        public string Password { get; set; } = "";
        public string Message { get; private set; } = "�������������!";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            Message = $"�����: {Login}, ������: {Password}";
        }
    }
}
