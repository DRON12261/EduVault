using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EduVault.Pages.Users
{
    public class UserModel : PageModel
    {
        private IUserService _userService;
        private IRoleService _roleService;
        public UserModel(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; } // "create", "edit"

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; } // Для редактирования
        public List<Role> Roles { get; set; }
        [BindProperty]
        public UserDTO Input { get; set; }

        public IActionResult OnGet()
        {
            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Roles = await _roleService.GetAllAsync();
            if (Mode == "create")
            {
                Input = new UserDTO(); // Инициализация пустой модели для создания
            }
            else if (Mode == "edit")
            {
                Input = new UserDTO(await _userService.GetByIdAsync(Id));// Загрузка данных пользователя по Id
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            if (Mode == "create")
            {
                await _userService.CreateAsync(Input);
            }
            else if (Mode == "edit")
            {
                await _userService.UpdateAsync(Input);
            }
            
            return RedirectToPage("/Users/Index");
        }
    }
}
