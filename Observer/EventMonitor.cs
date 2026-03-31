namespace MonitoringSystem.Observer;

/// <summary>
/// Делегат для обработчиков событий мониторинга.
/// Определяет сигнатуру: принимает MetricEventArgs, ничего не возвращает.
/// </summary>
public delegate void MetricEventHandler(MetricEventArgs e);

/// <summary>
/// Субъект (Subject / Издатель) паттерна «Наблюдатель».
///
/// Вместо классических методов Attach/Detach используются встроенные
/// механизмы C#: ключевое слово event и операторы += / -=.
/// Компилятор автоматически делает снимок списка подписчиков перед вызовом,
/// что защищает от состояния гонки (Race Condition) при переборе.
/// </summary>
public class EventMonitor
{
    /// <summary>
    /// Событие, на которое подписываются обработчики (наблюдатели).
    /// Оператор ?. в Invoke гарантирует безопасный вызов, даже если
    /// подписчиков нет.
    /// </summary>
    public event MetricEventHandler? OnMetricExceeded;

    /// <summary>
    /// Проверяет значение метрики относительно порога.
    /// Если значение превышает порог — публикует событие.
    /// </summary>
    /// <param name="metricName">Название метрики</param>
    /// <param name="value">Текущее значение</param>
    /// <param name="threshold">Критический порог</param>
    public void CheckMetric(string metricName, double value, double threshold)
    {
        Console.WriteLine($"[Monitor] Проверка {metricName}: {value:F1} (порог: {threshold:F1})");

        if (value > threshold)
        {
            // Создаём объект с данными метрики
            var eventData = new MetricData(metricName, value, threshold, DateTime.Now);

            // Публикуем событие всем подписчикам
            // ?.Invoke — потокобезопасный вызов: компилятор делает копию делегата
            OnMetricExceeded?.Invoke(
                new MetricEventArgs(eventType: metricName + "_Exceeded", data: eventData));
        }
    }
}
