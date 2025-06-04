using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduVault.Pages
{
    [Authorize]   
    public class TableModel : PageModel
    {
        public List<TableItem> Items { get; set; } = new();

        public void OnGet()
        {
            /*for (int i = 1; i <= 50; i++)
            {
                Items.Add(new TableItem
                {
                    Id = i,
                    Title = $"Документ {i}",
                    FileType = i % 2 == 0 ? "Тип файла 1" : "Тип файла 2",
                    CreationDate = DateTime.Now.AddDays(-i)
                });
            }*/
        }

        public class TableItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string FileType { get; set; }
            public DateTime CreationDate { get; set; }
        }
    }
}
