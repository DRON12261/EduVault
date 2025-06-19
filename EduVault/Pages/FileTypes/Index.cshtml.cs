using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EduVault.Models;
using EduVault.Data;
using Npgsql.TypeMapping;
using Microsoft.AspNetCore.Authorization;
using EduVault.Models.DataTransferObjects;

namespace EduVault.Pages.FileTypes
{
    [Authorize(Roles = "Администратор")]
    public class IndexModel : PageModel
    {
        private IFileTypeService _service;
        public List<FileType> FileTypes { get; set; } = new List<FileType>();
        [BindProperty(SupportsGet = true)]
        public FilterModel Filters { get; set; } = new FilterModel();
        public IndexModel(IFileTypeService service)
        {
            _service = service;
        }
        public async Task OnGetAsync()
        {
            FileTypes = await _service.GetFilteredRecordsAsync(Filters) ?? new();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long Id)
        {
            OperationResult result = await _service.DeleteById(Id);

            if (!(result.StatusCode == OperationStatusCode.Success))
            {
                TempData["ResultMessage"] = result.Message;
                return RedirectToPage();
            }

            TempData["ResultMessage"] = "Тип файлов успешно удалён";
            return RedirectToPage();
        }
    }
}
