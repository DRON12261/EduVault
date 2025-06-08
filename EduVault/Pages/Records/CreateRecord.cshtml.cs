using EduVault.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EduVault.Pages.Records
{
    public class CreateRecordModel : PageModel
    {
        private IRecordService _recordService;
        public CreateRecordModel(IRecordService recordService)
        {
            _recordService = recordService;
            Input = new InputModel();
        }
        [BindProperty]
        public InputModel Input { get; set; }
        //public FileType FileType { get; set; }
        public class InputModel
        {
            public string Name { get; set; }
            [Required]
            public string FileType { get; set; }
        }
        public void OnGet()
        {
        }
    }
}
