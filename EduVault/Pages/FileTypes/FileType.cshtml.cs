using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduVault.Pages.FileTypes
{
    [Authorize(Roles = "Администратор")]
    public class FileTypeModel : PageModel
    {
        private readonly IFileTypeService _fileTypeService;
        private readonly IFileTypeFieldService _fileTypeFieldService;

        public FileTypeModel(
            IFileTypeService fileTypeService,
            IFileTypeFieldService fileTypeFieldService)
        {
            _fileTypeService = fileTypeService;
            _fileTypeFieldService = fileTypeFieldService;
        }

        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public FileTypeDTO Input { get; set; }

        public List<FileTypeFieldDTO> Fields { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (Mode == "create")
            {
                Input = new FileTypeDTO();
                Fields = new List<FileTypeFieldDTO>();
            }
            else if (Mode == "edit")
            {
                if (Id == 0 && TempData["SavedId"] is string savedIdStr && long.TryParse(savedIdStr, out var savedId))
                {
                    Id = savedId;
                    TempData.Keep("SavedId");
                }
                TempData["SavedId"] = Id.ToString();
                Input = new FileTypeDTO(await _fileTypeService.GetByIdAsync(Id));
                if (Input == null)
                    return NotFound();
                Fields = (await _fileTypeFieldService.GetFieldsForFileTypeAsync(Id)).ToList();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (Mode == "create")
            {
                var createdId = await _fileTypeService.CreateAsync(Input);
                return RedirectToPage("./FileType", new { mode = "edit", id = createdId });
            }

            if (Mode == "edit")
            {
                await _fileTypeService.UpdateAsync(Input);
            }

            return RedirectToPage("/FileTypes/Index");
        }
    }
}
