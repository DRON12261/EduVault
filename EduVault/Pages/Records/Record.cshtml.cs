using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace EduVault.Pages.Records
{
    public class RecordModel : PageModel
    {
        private IRecordService _recordService;
        private IFileTypeService _fileTypeService;
        private IUserService _userService;
        public RecordModel(IRecordService recordService, IFileTypeService fileTypeService, IUserService userService)
        {
            _recordService = recordService;
            _fileTypeService = fileTypeService;
            _userService = userService;
        }
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; } // "create", "edit"

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; } // Для редактирования
        public List<FileType> FileTypes { get; set; } = new();
        public List<Field> Fields { get; set; } = new();
        public User Author { get; set; }
        [BindProperty]
        public RecordDTO Input { get; set; }
        public IActionResult OnGet()
        {
            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            FileTypes = await _fileTypeService.GetAllAsync();
            Author = await _userService.GetByLoginAsync(User.Identity.Name);
            if (Mode == "create")
            {
                Input = new RecordDTO(); // Инициализация пустой модели для создания
            }
            else if (Mode == "edit")
            {
                Input = new RecordDTO(await _recordService.GetByIdAsync(Id));// Загрузка данных пользователя по Id
            }
            return Page();
        }
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
               .Where(x => x.Value.Errors.Count > 0)
               .Select(x => new { x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
               .ToList();

                Console.WriteLine("Validation errors:");
                foreach (var error in errors)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Errors)}");
                }

                FileTypes = await _fileTypeService.GetAllAsync();
                return Page();
            }
            if (Mode == "create")
            {
                Input.RecordAuthor = (await _userService.GetByLoginAsync(User.Identity.Name)).Id;
                Input.RecordCreationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                await _recordService.CreateAsync(Input);
            }
            else if (Mode == "edit")
            {
                Input.RecordCreationDate = DateTime.SpecifyKind(Input.RecordCreationDate, DateTimeKind.Utc);
                await _recordService.UpdateAsync(Input);
            }
            return RedirectToPage("/Records/Index");
        }
    }
}
