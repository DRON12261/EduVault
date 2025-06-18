using System.Text.RegularExpressions;

namespace EduVault.Services
{
    public interface IFileNameTemplateService
    {
        bool ValidateFileName(string fileName, string template);
        Dictionary<string, string> ExtractValuesFromFileName(string fileName, string template);
        string GenerateFileName(Dictionary<string, string> values, string template);
    }

    public class FileNameTemplateService : IFileNameTemplateService
    {
        public bool ValidateFileName(string fileName, string template)
        {
            try
            {
                var regexPattern = ConvertTemplateToRegex(template);
                return Regex.IsMatch(Path.GetFileNameWithoutExtension(fileName), regexPattern);
            }
            catch
            {
                return false;
            }
        }

        public class UniversalTemplateConverter
        {
            public static string ConvertToRegex(string template)
            {
                // Экранируем весь шаблон, но сохраняем плейсхолдеры
                string escapedTemplate = Regex.Escape(template)
                    .Replace(@"\{", "{")  // Отменяем экранирование {
                    .Replace(@"\}", "}"); // Отменяем экранирование }

                // Заменяем все {Плейсхолдеры} на именованные группы
                string regexPattern = Regex.Replace(escapedTemplate,
                    @"\{([^{}]+)\}",    // Ищем {ЛюбойТекст}
                    match => $"(?<{SanitizeGroupName(match.Groups[1].Value)}>.+)"); // Преобразуем в (?<group>.+)

                return $"^{regexPattern}$";
            }

            // Очищаем имя группы для regex (убираем запрещённые символы)
            private static string SanitizeGroupName(string name)
            {
                return Regex.Replace(name, @"[^\w]", "_"); // Заменяем спецсимволы на _
            }
        }
        private string ConvertTemplateToRegex(string template)
        {
            // Преобразуем шаблон в regex
            // Пример: "ВКР_{Автор}_{Год}" → "^ВКР_(.+)_(\d{4})$"
            // 1. Сначала заменяем плейсхолдеры на regex-группы
            /*string pattern = template
                .Replace("{Автор}", "(?<author>.+)")
                .Replace("{Тип}", "(?<type>[ИК])")
                .Replace("{Код}", @"(?<code>\d{2}\.\d{2}\.\d{2})")
                .Replace("{Год}", @"(?<year>\d{4})");*/

            // 2. Экранируем только оставшиеся спецсимволы
            /*string regexPattern = Regex.Escape(pattern)
                .Replace(@"\(?<author>\.\+\)", "(?<author>.+)")
                .Replace(@"\(?<type>\.\+\)", "(?<type>.+)")
                .Replace(@"\(?<code>\.\+\)", "(?<code>.+)")
                .Replace(@"\(?<year>\d\{4\}\)", @"(?<year>\d{4})");*/
            string regex = ConvertToRegex(template);

            return $"^{regex}$";
        }

        public static string ConvertToRegex(string template)
        {
            // Экранируем весь шаблон, но сохраняем плейсхолдеры
            string escapedTemplate = Regex.Escape(template)
                .Replace(@"\{", "{")  // Отменяем экранирование {
                .Replace(@"\}", "}"); // Отменяем экранирование }

            // Заменяем все {Плейсхолдеры} на именованные группы
            string regexPattern = Regex.Replace(escapedTemplate,
                @"\{([^{}]+)\}",    // Ищем {ЛюбойТекст}
                match => $"(?<{SanitizeGroupName(match.Groups[1].Value)}>{GetPatternForPlaceholder(match.Groups[1].Value)})"); // Преобразуем в (?<group>.+)

            return $"{regexPattern}";
        }

        // Очищаем имя группы для regex (убираем запрещённые символы)
        private static string SanitizeGroupName(string name)
        {
            return Regex.Replace(name, @"[^\w]", "_"); // Заменяем спецсимволы на _
        }
        private static string GetPatternForPlaceholder(string placeholder)
        {
            return placeholder switch
            {
                "Год" => "\\d{4}",
                "Версия" => "v\\d+\\.\\d+",
                "Направление" => "\\d{2}\\.\\d{2}\\.\\d{2}",
                "Тип" => "[ИК]",
                _ => ".+" // По умолчанию - любые символы
            };
        }
        public Dictionary<string, string> ExtractValuesFromFileName(string fileName, string template)
        {
            var regex = new Regex(ConvertTemplateToRegex(template));
            var match = regex.Match(Path.GetFileNameWithoutExtension(fileName));

            if (!match.Success)
                return null;

            return match.Groups
                .OfType<Group>()
                .Skip(1)
                .ToDictionary(g => g.Name, g => g.Value);
        }

        public string GenerateFileName(Dictionary<string, string> values, string template)
        {
            foreach (var kvp in values)
            {
                template = template.Replace($"{{{kvp.Key}}}", kvp.Value);
            }
            return template;
        }
    }
}
