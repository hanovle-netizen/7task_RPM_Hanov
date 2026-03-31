using MonitoringSystem.Observer;
using MonitoringSystem.Strategy;

namespace MonitoringSystem.TemplateMethod;

/// <summary>
/// Абстрактный базовый класс (AbstractClass) паттерна «Шаблонный метод».
/// Одновременно является Контекстом (Context) паттерна «Стратегия».
///
/// Шаблонный метод ProcessEvent фиксирует алгоритм обработки события:
///   1. FormatMessage — форматирование сообщения (делегируется стратегии)
///   2. SendMessage   — отправка уведомления (реализуется подклассом)
///   3. LogResult     — логирование результата (hook-метод, опционален)
/// </summary>
public abstract class EventHandlerBase
{
    // === Часть паттерна «Стратегия» ===

    /// <summary>Текущая стратегия форматирования сообщений</summary>
    protected IFormatStrategy _formatStrategy;

    protected EventHandlerBase(IFormatStrategy strategy)
    {
        _formatStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    /// <summary>
    /// Позволяет менять стратегию форматирования во время выполнения
    /// без изменения кода обработчика.
    /// </summary>
    public void SetStrategy(IFormatStrategy strategy)
    {
        _formatStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    // === Часть паттерна «Шаблонный метод» ===

    /// <summary>
    /// Шаблонный метод — определяет скелет алгоритма обработки события.
    /// Последовательность шагов зафиксирована; детали реализуют подклассы.
    /// </summary>
    public void ProcessEvent(MetricEventArgs e)
    {
        // Шаг 1: форматирование через стратегию (абстрактный шаг)
        var message = FormatMessage(e.EventType, e.Data);

        // Шаг 2: отправка уведомления (абстрактный шаг — канал доставки)
        SendMessage(message);

        // Шаг 3: логирование (hook-метод — подкласс может переопределить)
        LogResult();
    }

    /// <summary>
    /// Абстрактный шаг: формирует текст сообщения, используя стратегию.
    /// Подкласс определяет, какие данные включить в текст.
    /// </summary>
    protected abstract string FormatMessage(string type, object data);

    /// <summary>
    /// Абстрактный шаг: отправляет отформатированное сообщение в канал доставки.
    /// Каждый подкласс реализует свой канал (консоль, файл, email и т.д.).
    /// </summary>
    protected abstract void SendMessage(string message);

    /// <summary>
    /// Hook-метод: опциональный шаг логирования результата.
    /// По умолчанию ничего не делает; подкласс может переопределить.
    /// </summary>
    protected virtual void LogResult() { }
}
