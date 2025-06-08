using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EduVault.Pages.FileTypes
{
    public class CreateFileTypeModel : PageModel
    {
        private IFileTypeService _fileTypeService;
        public CreateFileTypeModel(IFileTypeService fileTypeService)
        {
            _fileTypeService = fileTypeService;
            Input = new InputModel();
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            public string Name { get; set; }
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            await _fileTypeService.CreateFileTypeAsync(Input.Name);
            return RedirectToPage("/FileTypes/Index");
        }
    }
}
