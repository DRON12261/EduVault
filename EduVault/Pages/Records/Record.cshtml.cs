using EduVault.Models;
using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System.Data;
using System.Text.Json;
using System.Net;

namespace EduVault.Pages.Records
{
    [Authorize]
    public class RecordModel : PageModel
    {
        private IRecordService _recordService;
        private IFileTypeService _fileTypeService;
        private IUserService _userService;
        private IMongoFileService _mongoFileService;
        private IRelationService _relationService;
        private IFileNameTemplateService _fileNameTemplateService;
        private IFileTypeFieldService _fileTypeFieldService;
        private IFieldService _fieldService;
        //private IAccessRightsService _accessRightsService;
        public RecordModel(
            IRecordService recordService,
            IFileTypeService fileTypeService,
            IUserService userService,
            IMongoFileService mongoFileService,
            IRelationService relationService,
            IFileNameTemplateService fileNameTemplateService,
            IFileTypeFieldService fileTypeFieldService,
            IFieldService fieldService
            /*, IAccessRightsService accessRightsService*/)
        {
            _recordService = recordService;
            _fileTypeService = fileTypeService;
            _userService = userService;
            _mongoFileService = mongoFileService;
            _relationService = relationService;
            _fileNameTemplateService = fileNameTemplateService;
            _fileTypeFieldService = fileTypeFieldService;
            _fieldService = fieldService;
            //_accessRightsService = accessRightsService;
        }
        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; } // "create", "edit"

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; } // Для редактирования
        public List<FileType> FileTypes { get; set; } = new();
        public List<FileTypeFieldDTO> FileTypeFields { get; set; } = new();
        public List<Field> Fields { get; set; } = new();

        //public List<AccessRightsDTO> AccessRights { get; set; } = new();
        public User Author { get; set; }
        [BindProperty]
        public RecordDTO Input { get; set; }
        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        [BindProperty]
        public List<long> SelectedRelationships { get; set; } = new(); // Выбранные связи
        public bool CanPreviewFile { get; set; }

        public List<Record> AvailableRecords { get; set; } = new();
        public List<Relation> CurrentRelations { get; set; } = new();
        public async Task<IActionResult> OnGetAsync()
        {
            FileTypes = await _fileTypeService.GetAllAsync();

            if (Mode == "edit")
            {
                //CurrentRelations = await _relationService.GetRelationshipsForRecordAsync(Id);
            }
            AvailableRecords = (await _recordService.GetAllAsync())
                              .Where(r => r.Id != Id) // Исключаем текущую запись
                              .ToList();
            if (Mode == "create")
            {
                Author = await _userService.GetByLoginAsync(User.Identity.Name);
                Input = new RecordDTO(); // Инициализация пустой модели для создания
                                         // Если сразу выбран тип файла (например, из query string)
                if (Input.FileType > 0)
                {
                    await LoadFieldTemplates(Input.FileType);
                }
            }
            else if (Mode == "edit")
            {
                //AccessRights = await _accessRightsService.GetAccessRightsForRecordAsync(Id);
                Input = new RecordDTO(await _recordService.GetByIdAsync(Id));// Загрузка данных пользователя по Id
                Input.FileName = await _mongoFileService.GetFileNameAsync(Input.FilePath);
                Author = await _userService.GetByIdAsync(Input.RecordAuthor);
                // Загружаем шаблоны полей для типа файла
                await LoadFieldTemplates(Input.FileType);
                // Загружаем значения полей
                Input.CustomFieldsValues = await _fieldService.GetFieldsForRecordAsync(Id);
            }
            if (Input?.FileType > 0)
            {
                FileTypeFields = await _fileTypeFieldService.GetFieldsForFileTypeAsync(Input.FileType);
            }
            if (Mode == "edit" && Id > 0)
            {
                // Проверяем, можно ли просматривать файл
                CanPreviewFile = CanFileBePreviewed(Input.FileName);
            }
            //TempData["InputData"] = JsonSerializer.Serialize(Input);
            TempData["SavedId"] = JsonSerializer.Serialize(Id);
            return Page();
        }
        private async Task LoadFieldTemplates(long fileTypeId)
        {
            FileTypeFields = await _fileTypeFieldService.GetFieldsForFileTypeAsync(fileTypeId);
            Input.CustomFieldsTemplate = FileTypeFields.ToDictionary(
                f => f.Id,
                f => f.DefaultValue ?? ""); // Используем DefaultValue из FileTypeField или пустую строку
        }
        public async Task<IActionResult> OnGetDownloadFileAsync()
        {
            /*if(TempData["InputData"] is RecordDTO serializedInput){
                Input = serializedInput;
            }*/
            if (TempData["SavedId"] is string savedIdStr && long.TryParse(savedIdStr, out var savedId))
            {
                Id = savedId;
                TempData.Keep("SavedId");
            }
            var record = await _recordService.GetByIdAsync(Id);
            if (record == null) return NotFound();

            var (stream, contentType, fileName, error) = await _mongoFileService.DownloadFileAsync(record.FilePath);
            if (stream == null) return NotFound(error);

            // Определяем, можно ли открывать файл в браузере
            CanPreviewFile = CanFileBePreviewed(Path.GetExtension(fileName)?.ToLower());

            // Устанавливаем правильные заголовки
            Response.Headers.Append("Content-Disposition",
                CanPreviewFile
                    ? $"inline; filename=\"{WebUtility.UrlEncode(fileName)}\""
                    : $"attachment; filename=\"{WebUtility.UrlEncode(fileName)}\"");

            // Для известных типов используем их Content-Type, для остальных - общий
            return File(stream,
                CanPreviewFile ? contentType : "application/octet-stream",
                CanPreviewFile ? null : fileName); // null для inline отключает принудительное скачивание
        }
        private bool CanFileBePreviewed(string fileId)
        {
            var extension = Path.GetExtension(fileId)?.ToLower();
            return new[] { ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".txt", ".webp" }
                .Contains(extension);
        }

        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        public async Task<IActionResult> OnPostSaveRelationshipsAsync()
        {
            // Удаляем старые связи
            var existing = await _relationService.GetRelationshipsForRecordAsync(Id);
            foreach (var rel in existing)
            {
                await _relationService.DeleteRelationshipAsync(rel.Id);
            }

            // Добавляем новые
            foreach (var targetId in SelectedRelationships)
            {
                await _relationService.CreateRelationshipAsync(new Relation(Id, targetId));
            }

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (Input?.FileType > 0)
            {
                FileTypeFields = await _fileTypeFieldService.GetFieldsForFileTypeAsync(Input.FileType);
            }

            // Валидация обязательных полей
            foreach (var field in FileTypeFields.Where(f => f.IsRequired))
            {
                if (string.IsNullOrWhiteSpace(Input.CustomFieldsTemplate.GetValueOrDefault(field.Id)))
                {
                    ModelState.AddModelError($"Input.CustomFields[{field.Id}]",
                        $"Поле '{field.Name}' обязательно для заполнения");
                }
            }
            string old_filepath = "";
            ModelState.Remove("Input.FilePath");
            ModelState.Remove("Input.FileName");
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
            string uploadedFilePath = null;

            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                // Получаем шаблон для типа файла
                var template = (await _fileTypeService.GetByIdAsync(Input.FileType)).FileNameTemplate;

                // Проверяем имя файла
                if (!string.IsNullOrEmpty(template) &&
                    !_fileNameTemplateService.ValidateFileName(UploadedFile.FileName, template))
                {
                    ModelState.AddModelError("", $"Имя файла не соответствует шаблону: {template}");
                    return Page();
                }

                // Если имя файла соответствует шаблону, извлекаем данные
                if (!string.IsNullOrEmpty(template))
                {
                    var extractedData = _fileNameTemplateService.ExtractValuesFromFileName(
                        UploadedFile.FileName,
                        template);

                    // Заполняем поля карточки на основе извлеченных данных
                    if (extractedData != null)
                    {
                        /*if (extractedData.TryGetValue("author", out var author))
                            Input.Author = author;*/

                        // ... заполнение других полей ...
                        try
                        {
                            using var stream = UploadedFile.OpenReadStream();
                            uploadedFilePath = await _mongoFileService.UploadFileAsync(
                                stream,
                                UploadedFile.FileName,
                                UploadedFile.ContentType);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", $"Ошибка загрузки файла: {ex.Message}");
                            FileTypes = await _fileTypeService.GetAllAsync();
                            return Page();
                        }
                    }
                }
                
            }
            if (!string.IsNullOrEmpty(uploadedFilePath))
            {
                if (Mode == "edit")
                {
                    old_filepath = (await _recordService.GetByIdAsync(Input.Id)).FilePath;
                }
                Input.FilePath = uploadedFilePath;
            }
            try
            {
                if (Mode == "create")
                {
                    Input.RecordAuthor = (await _userService.GetByLoginAsync(User.Identity.Name)).Id;
                    Input.RecordCreationDate = DateTime.UtcNow;
                    var recordId = await _recordService.CreateAsync(Input);
                    // Сохраняем значения полей
                    await _fieldService.SaveFieldsForRecordAsync(recordId, Input.CustomFieldsValues);
                    await _recordService.CreateAsync(Input);
                }
                else if (Mode == "edit")
                {
                    Input.RecordCreationDate = (await _recordService.GetByIdAsync(Input.Id)).RecordCreationDate;
                    // Обновляем значения полей
                    await _fieldService.SaveFieldsForRecordAsync(Input.Id, Input.CustomFieldsValues);
                    await _recordService.UpdateAsync(Input);
                    await _mongoFileService.DeleteFileAsync(old_filepath);
                }
            }
            catch (Exception ex)
            {
                // Если сохранение не удалось - удаляем загруженный файл
                if (!string.IsNullOrEmpty(uploadedFilePath))
                {
                    try { await _mongoFileService.DeleteFileAsync(uploadedFilePath); }
                    catch { /* Игнорируем ошибки удаления */ }
                }

                ModelState.AddModelError("", $"Ошибка сохранения: {ex.Message}");
                FileTypes = await _fileTypeService.GetAllAsync();
                return Page();
            }
            return RedirectToPage("/Records/Index");
        }
    }
}
