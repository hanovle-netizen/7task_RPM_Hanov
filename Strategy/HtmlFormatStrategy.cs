namespace MonitoringSystem.Strategy;

/// <summary>
/// Конкретная стратегия: форматирование в HTML-разметку.
/// Пример вывода: &lt;div class="alert"&gt;...&lt;/div&gt;
/// </summary>
public class HtmlFormatStrategy : IFormatStrategy
{
    public string Format(string message, DateTime timestamp)
    {
        // Экранируем спецсимволы HTML
        var safeMessage = message
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");

        return $"<div class=\"alert\">" +
               $"<span class=\"timestamp\">{timestamp:yyyy-MM-dd HH:mm:ss}</span> " +
               $"<span class=\"message\">{safeMessage}</span>" +
               $"</div>";
    }
}
