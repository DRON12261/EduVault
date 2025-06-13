using EduVault.Models;
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

        public List<Record> Records { get; set; } = new();
        public List<FileType> FileTypes { get; set; } = new();
        public List<User> Users { get; set; } = new();

        public IndexModel(IRecordService recordService, IFileTypeService fileTypeService, IUserService userService)
        {
            _recordService = recordService;
            _fileTypeService = fileTypeService;
            _userService = userService;
        }
        public async Task OnGetAsync()
        {
            Users = await _userService.GetAllAsync() ?? new();
            FileTypes = await _fileTypeService.GetAllAsync() ?? new();
            Records = await _recordService.GetAllAsync() ?? new();
        }
        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            OperationResult result = await _recordService.DeleteById(id);

            if (!(result.StatusCode == OperationStatusCode.Success))
            {
                TempData["ResultMessage"] = result.Message;
                return RedirectToPage();
            }

            TempData["ResultMessage"] = "Пользователь успешно удалён";
            return RedirectToPage();
        }
    }
}
