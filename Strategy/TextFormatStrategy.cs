namespace MonitoringSystem.Strategy;

/// <summary>
/// Конкретная стратегия: форматирование в простой текст.
/// Пример вывода: [2026-03-31 14:30:00] [CPU_Load_Exceeded] Metric: CPU_Load, Value: 95.0
/// </summary>
public class TextFormatStrategy : IFormatStrategy
{
    public string Format(string message, DateTime timestamp) =>
        $"[{timestamp:yyyy-MM-dd HH:mm:ss}] {message}";
}
