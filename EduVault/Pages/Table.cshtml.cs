using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduVault.Pages
{
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
            Items.Add(new TableItem
            {
                Id = 6,
                Title = "Презентация \"Создание web-приложения\" Скочко А.Е., Чипак Д.И., 2024",
                FileType = "Презентация",
                CreationDate = DateTime.Parse("2024-02-17")
            });
            Items.Add(new TableItem
            {
                Id = 7,
                Title = "Исходный код игры \"Три в Ряд\" Чипак Д.И., 2020",
                FileType = "Программный код",
                CreationDate = DateTime.Parse("2020-12-05")
            });
            Items.Add(new TableItem
            {
                Id = 8,
                Title = "Курс.Р \"Разработка игры Три в ряд\" Чипак Д.И., 2020",
                FileType = "Отчет по курсовой работе",
                CreationDate = DateTime.Parse("2020-12-05")
            });
            Items.Add(new TableItem
            {
                Id = 12,
                Title = "Презентация \"Разработка приложения для построения 3D моделей с помощью трассировки лучей\" Скочко А.Е., Чипак Д.И., Ибраев Е.И. 2024",
                FileType = "Презентация",
                CreationDate = DateTime.Parse("2024-01-21")
            });
            Items.Add(new TableItem
            {
                Id = 13,
                Title = "ВКР \"Разработка системы проведения контрольных мероприятий в образовательных организациях Тюменской области\" Скочко А.Е., Чипак Д.И., 2023",
                FileType = "Отчет по ВКР",
                CreationDate = DateTime.Parse("2023-10-13")
            });
            Items.Add(new TableItem
            {
                Id = 15,
                Title = "Курс.Р \"Трассировка лучей\" Скочко А.Е., Чипак Д.И., Ибраев Е.И., 2019",
                FileType = "Отчет по курсовой работе",
                CreationDate = DateTime.Parse("2019-09-30")
            });
            Items.Add(new TableItem
            {
                Id = 27,
                Title = "Курсовая \"Проверка отчётов студентов на соответствие требованиям\"",
                FileType = "Отчет по курсовой работе",
                CreationDate = DateTime.Parse("2021-06-15")
            });
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
