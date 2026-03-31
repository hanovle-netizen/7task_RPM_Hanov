namespace MonitoringSystem.Observer;

/// <summary>
/// Аргументы события мониторинга.
/// Содержит тип события и данные метрики.
/// Наследует EventArgs — стандартное соглашение .NET для аргументов событий.
/// </summary>
public class MetricEventArgs(string eventType, MetricData data) : EventArgs
{
    /// <summary>Тип события (например, "CPU_Load_Exceeded")</summary>
    public string EventType { get; } = eventType
        ?? throw new ArgumentNullException(nameof(eventType));

    /// <summary>Данные метрики, вызвавшей событие</summary>
    public MetricData Data { get; } = data
        ?? throw new ArgumentNullException(nameof(data));
}
