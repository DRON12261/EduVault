using EduVault.Models.DataTransferObjects;
using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EduVault.Pages.Records
{
    public class RelationsModel : PageModel
    {
        private readonly IRecordService _recordService;
        private readonly IRelationService _relationService;
        public RelationsModel(IRecordService recordService, IRelationService relationService)
        {
            _recordService = recordService;
            _relationService = relationService;
        }
        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public string RecordName { get; set; }
        public List<RecordDTO> Relations { get; set; }
        public List<RecordDTO> AvailableRecords { get; set; }
        public async Task OnGetAsync()
        {
            await LoadData();
        }

        public async Task<IActionResult> OnPostAddRelation(long recordId)
        {
            await _relationService.CreateRelationshipAsync(new Models.Relation(Id,recordId));
            await LoadData();
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveRelation(long recordId)
        {
            await _relationService.DeleteRelationshipAsync(new Models.Relation(Id, recordId));
            await LoadData();
            return Page();
        }

        private async Task LoadData()
        {
            var record = await _recordService.GetByIdAsync(Id);
            RecordName = record.Name;

            Relations = (await _relationService.GetRelatedRecordsForRecordAsync(Id)).Select(r=>new RecordDTO(r)).ToList();
            var allRecords = (await _recordService.GetAllAsync()).Select(r => new RecordDTO(r)).ToList();
            AvailableRecords = allRecords
                .Where(rec => !Relations.Any(rel => rel.Id == rec.Id) && rec.Id!=Id)
                .ToList();
        }
    }
}
