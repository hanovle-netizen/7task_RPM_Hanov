namespace MonitoringSystem.Strategy;

/// <summary>
/// Интерфейс стратегии форматирования сообщений (Strategy).
///
/// Определяет общий контракт для всех конкретных стратегий.
/// Контекст (EventHandlerBase) работает только с этим интерфейсом,
/// что позволяет менять стратегию без изменения кода контекста.
/// </summary>
public interface IFormatStrategy
{
    /// <summary>
    /// Форматирует сообщение в нужный формат.
    /// </summary>
    /// <param name="message">Текст сообщения</param>
    /// <param name="timestamp">Временная метка события</param>
    /// <returns>Отформатированная строка</returns>
    string Format(string message, DateTime timestamp);
}
