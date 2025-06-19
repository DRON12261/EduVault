using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduVault.Pages.Records
{
    [Authorize]   
    public class IndexModel : PageModel
    {
        private IRecordService _recordService;
        private IFileTypeService _fileTypeService;
        private IUserService _userService;
        private IMongoFileService _mongoFileService;
        public List<Record> Records { get; set; } = new();
        public List<FileType> FileTypes { get; set; } = new();
        public List<User> Users { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public FilterModel Filters { get; set; } = new FilterModel();
        public IndexModel(IRecordService recordService, IFileTypeService fileTypeService, IUserService userService, IMongoFileService mongoFileService)
        {
            _recordService = recordService;
            _fileTypeService = fileTypeService;
            _userService = userService;
            _mongoFileService = mongoFileService;
        }

        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllAsync() ?? new();
            FileTypes = await _fileTypeService.GetAllAsync() ?? new();
            Records = await _recordService.GetFilteredRecordsAsync(Filters) ?? new();
        }
        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            string filepath = (await _recordService.GetByIdAsync(id)).FilePath;
            OperationResult result = await _recordService.DeleteById(id);

            if (!(result.StatusCode == OperationStatusCode.Success))
            {
                TempData["ResultMessage"] = result.Message;
                return RedirectToPage();
            }

            await _mongoFileService.DeleteFileAsync(filepath);
            TempData["ResultMessage"] = "Карточка успешно удалена";
            return RedirectToPage();
        }
    }
}
