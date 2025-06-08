using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EduVault.Pages.Users
{
    public class CreateUserModel : PageModel
    {
        private IUserService _userService;
        public CreateUserModel(IUserService userService)
        {
            _userService = userService;
            Input = new InputModel();
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Name { get; set; }
            [Required]
            public string Login { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            await _userService.CreateUserAsync(new UserCreationDto(Input.Name, Input.Login, Input.Password));
            return RedirectToPage("/Users/Index");
        }
    }
}
