using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EduVault.Models;
using EduVault.Data;
using Npgsql.TypeMapping;

namespace EduVault.Pages.FileTypes
{
    public class IndexModel : PageModel
    {
        private IFileTypeService _fileTypeService;
        public List<FileType> FileTypes { get; set; } = new List<FileType>();
        public IndexModel(IFileTypeService fileTypeService)
        {
            _fileTypeService = fileTypeService;
        }
        public async Task OnGetAsync()
        {
            FileTypes = await _fileTypeService.GetAllFileTypesAsync() ?? new List<FileType>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            OperationResult result = await _fileTypeService.DeleteFileTypeById(id);

            if (!(result.StatusCode == OperationStatusCode.Success))
            {
                TempData["ResultMessage"] = result.ErrorMessage;
                return RedirectToPage();
            }

            TempData["ResultMessage"] = "Тип успешно удалён";
            return RedirectToPage();
        }
    }
}
