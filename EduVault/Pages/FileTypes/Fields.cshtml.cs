using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EduVault.Pages.FileTypes
{
    [Authorize(Roles = "Администратор")]
    public class FieldsModel : PageModel
    {
        private readonly IFileTypeService _fileTypeService;
        private readonly IFileTypeFieldService _fileTypeFieldService;

        public FieldsModel(
            IFileTypeService fileTypeService,
            IFileTypeFieldService fileTypeFieldService)
        {
            _fileTypeService = fileTypeService;
            _fileTypeFieldService = fileTypeFieldService;
        }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        [BindProperty]
        public FileTypeFieldDTO NewField { get; set; } = new();

        public List<FileTypeFieldDTO> Fields { get; set; } = new();
        public string FileTypeName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var fileType = await _fileTypeService.GetByIdAsync(Id);
            if (fileType == null)
                return NotFound();

            FileTypeName = fileType.Name;
            Fields = (await _fileTypeFieldService.GetFieldsForFileTypeAsync(Id)).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAddFieldAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            NewField.FileTypeId = Id;
            await _fileTypeFieldService.AddFieldToFileTypeAsync(new FileTypeField(NewField));
            Fields = (await _fileTypeFieldService.GetFieldsForFileTypeAsync(Id)).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFieldAsync(long fieldId)
        {
            await _fileTypeFieldService.RemoveFieldFromFileTypeAsync(fieldId);
            Fields = (await _fileTypeFieldService.GetFieldsForFileTypeAsync(Id)).ToList();
            return Page();
        }
    }
}
