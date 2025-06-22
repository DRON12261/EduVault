using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

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
        public FileTypeDTO Input { get; set; } = new();

        [BindProperty]
        public List<FileTypeFieldDTO> Fields { get; set; } = new();

        [BindProperty]
        public FileTypeFieldDTO NewField { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ModelState.Remove("Name");
            ModelState.Remove("FileNameTemplate");
            if (Mode == "create")
            {
                Input = new FileTypeDTO();
            }
            else if (Mode == "edit")
            {
                TempData["SavedId"] = Id.ToString();
                Input = new FileTypeDTO(await _fileTypeService.GetByIdAsync(Id));
                if (Input == null)
                    return NotFound();
                TempData["InputData"] = JsonSerializer.Serialize(Input);
                Fields = await _fileTypeFieldService.GetFieldsForFileTypeAsync(Id);
                TempData["TempFields"] = JsonSerializer.Serialize(Fields);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddFieldAsync()
        {
            await LoadDataAsync();

            // Очищаем все ошибки валидации
            ModelState.Clear();

            // Валидируем только NewField
            var validationContext = new ValidationContext(NewField, null, null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(NewField, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var result in validationResults)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        ModelState.AddModelError($"{nameof(NewField)}.{memberName}", result.ErrorMessage);
                    }
                }
                return Page();
            }

            // Устанавливаем FileTypeId (для edit - текущий ID, для create - временный -1)
            NewField.FileTypeId = Mode == "edit" ? Id : -1;
            Fields.Add(NewField);
            TempData["TempFields"] = JsonSerializer.Serialize(Fields);
            NewField = new FileTypeFieldDTO();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Partial("_FieldsTablePartial", this);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFieldAsync(int index)
        {
            await LoadDataAsync(); // Загружаем актуальные данные

            if (index >= 0 && index < Fields.Count)
            {
                Fields.RemoveAt(index);
                TempData["TempFields"] = JsonSerializer.Serialize(Fields);
                TempData.Keep("TempFields"); // Явно сохраняем TempData
            }
            return Partial("_FieldsTablePartial", this);
        }

        public async Task<IActionResult> OnPostSaveFieldsAsync()
        {
            //await LoadDataAsync();
            TempData["TempFields"] = JsonSerializer.Serialize(Fields);
            //return RedirectToPage($"./FileType/edit?id={(string)TempData["SavedId"]}");
            //return Page();
            return Partial("_FieldsTablePartial", this);
        }
        public async Task<IActionResult> OnPostSaveCardAsync()
        {

            ModelState.Remove("Name");
            ModelState.Remove("FileNameTemplate");
            if (TempData["SavedId"] is string savedIdStr && long.TryParse(savedIdStr, out var savedId))
            {
                // Используем savedId (тип long)
                Id = savedId;
                //TempData.Keep("SavedId");
            }
            await LoadDataAsync();

            if (string.IsNullOrWhiteSpace(Input.Name))
            {
                ModelState.AddModelError($"{nameof(Input)}.{nameof(Input.Name)}", "Название типа файла обязательно");
            }

            foreach (var field in Fields)
            {
                if (string.IsNullOrWhiteSpace(field.Name))
                {
                    ModelState.AddModelError("", "Все добавленные поля должны иметь название");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                // Логируем все ошибки валидации
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => new
                    {
                        Field = x.Key,
                        Error = e.ErrorMessage
                    }))
                    .ToList();

                return Page();
            }

            Input.FileTypeFields = Fields.Where(f => !string.IsNullOrWhiteSpace(f.Name)).ToList();

            try
            {
                if (Mode == "create")
                {
                    var createdId = await _fileTypeService.CreateAsync(Input);
                    return RedirectToPage("./FileType", new { mode = "edit", id = createdId });
                }

                if (Mode == "edit")
                {
                    await _fileTypeService.UpdateAsync(Input);
                }

                return RedirectToPage("./Index");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }

        private async Task LoadDataAsync()
        {
            // Всегда восстанавливаем Id из TempData если есть
            if (TempData["SavedId"] is string savedIdStr && long.TryParse(savedIdStr, out var savedId))
            {
                Id = savedId;
                TempData.Keep("SavedId");
            }

            if (Mode == "edit")
            {
                if (Id > 0)
                {
                    Input ??= new FileTypeDTO(await _fileTypeService.GetByIdAsync(Id));
                    if (TempData["TempFields"] is string serializedFields)
                    {
                        try
                        {
                            Fields = JsonSerializer.Deserialize<List<FileTypeFieldDTO>>(serializedFields)
                                ?? await _fileTypeFieldService.GetFieldsForFileTypeAsync(Id);
                        }
                        catch
                        {
                            Fields = await _fileTypeFieldService.GetFieldsForFileTypeAsync(Id);
                        }
                        TempData.Keep("TempFields");
                    }
                    else
                    {
                        Fields = await _fileTypeFieldService.GetFieldsForFileTypeAsync(Id);
                    }
                }
                else
                {

                }

                // Восстанавливаем Fields из TempData или из БД
                
            }
            else if (Mode == "create")
            {
                Input ??= new FileTypeDTO();

                if (TempData["TempFields"] is string serializedFields)
                {
                    try
                    {
                        Fields = JsonSerializer.Deserialize<List<FileTypeFieldDTO>>(serializedFields)
                            ?? new List<FileTypeFieldDTO>();
                        TempData.Keep("TempFields");
                    }
                    catch
                    {
                        Fields = new List<FileTypeFieldDTO>();
                    }
                }
                else
                {
                    Fields = new List<FileTypeFieldDTO>();
                }
            }
        }
    }
}
