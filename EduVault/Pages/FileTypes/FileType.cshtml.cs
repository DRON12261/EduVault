using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EduVault.Pages.FileTypes
{
    [Authorize(Roles = "Администратор")]
    public class FileTypeModel : PageModel
    {
        private IFileTypeService _service;
        public FileTypeModel(IFileTypeService service)
        {
            _service = service;
        }

        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }
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
                Input = new FileTypeDTO();
            }
            else if (Mode == "edit")
            {
                Input = new FileTypeDTO(await _service.GetByIdAsync(Id));
            }
            return Page();
        }
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            if (Mode == "create")
            {
                await _service.CreateAsync(Input);
            }
            else if (Mode == "edit")
            {
                await _service.UpdateAsync(Input);
            }
            return RedirectToPage("./Index");
        }
    }
}
