namespace MonitoringSystem.Strategy;

/// <summary>
/// Конкретная стратегия: форматирование в JSON.
/// Пример вывода: {"timestamp":"2026-03-31T14:30:00","message":"[CPU_Load_Exceeded] ..."}
/// </summary>
public class JsonFormatStrategy : IFormatStrategy
{
    public string Format(string message, DateTime timestamp)
    {
        // Экранируем кавычки и обратные слэши в сообщении
        var escaped = message.Replace("\\", "\\\\").Replace("\"", "\\\"");
        return $"{{\"timestamp\":\"{timestamp:O}\",\"message\":\"{escaped}\"}}";
    }
}
