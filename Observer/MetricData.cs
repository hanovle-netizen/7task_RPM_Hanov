namespace MonitoringSystem.Observer;

/// <summary>
/// Класс, представляющий данные о событии мониторинга.
/// Передаётся вместе с уведомлением от издателя всем подписчикам.
/// </summary>
public class MetricData(string metricName, double value, double threshold, DateTime timestamp)
{
    /// <summary>Название метрики (CPU, Memory, Network и т.д.)</summary>
    public string MetricName { get; } = metricName
        ?? throw new ArgumentNullException(nameof(metricName));

    /// <summary>Зафиксированное значение метрики</summary>
    public double Value { get; } = value;

    /// <summary>Критический порог, при превышении которого генерируется событие</summary>
    public double Threshold { get; } = threshold;

    /// <summary>Время фиксации события</summary>
    public DateTime Timestamp { get; } = timestamp;

    public override string ToString() =>
        $"Metric: {MetricName}, Value: {Value:F1} (Threshold: {Threshold:F1})";
}
