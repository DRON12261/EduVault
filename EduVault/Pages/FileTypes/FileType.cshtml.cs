using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EduVault.Pages.FileTypes
{
    public class FileTypeModel : PageModel
    {
        private IFileTypeService _fileTypeService;
        public FileTypeModel(IFileTypeService fileTypeService)
        {
            _fileTypeService = fileTypeService;
        }

        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; } // "create", "edit"

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; } // Для редактирования
        [BindProperty]
        public FileTypeDTO Input { get; set; }

        public IActionResult OnGet()
        {
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Mode == "create")
            {
                Input = new FileTypeDTO(); // Инициализация пустой модели для создания
            }
            else if (Mode == "edit")
            {
                Input = new FileTypeDTO(await _fileTypeService.GetByIdAsync(Id));// Загрузка данных пользователя по Id
            }
            return Page();
        }
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            if (Mode == "create")
            {
                await _fileTypeService.CreateAsync(Input);
            }
            else if (Mode == "edit")
            {
                await _fileTypeService.UpdateAsync(Input);
            }
            return RedirectToPage("/FileTypes/Index");
        }
    }
}
