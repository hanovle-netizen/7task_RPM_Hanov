using MonitoringSystem.Strategy;

namespace MonitoringSystem.TemplateMethod;

/// <summary>
/// Конкретный обработчик (ConcreteClass): вывод уведомлений в консоль.
/// Реализует абстрактные шаги базового класса для канала «консоль».
/// </summary>
public class ConsoleHandler(string name, IFormatStrategy strategy) : EventHandlerBase(strategy)
{
    private readonly string _name = name;

    /// <summary>
    /// Реализация шага форматирования: строит текст сообщения
    /// и передаёт его в текущую стратегию форматирования.
    /// </summary>
    protected override string FormatMessage(string type, object data) =>
        _formatStrategy.Format($"[{type}] {data}", DateTime.Now);

    /// <summary>
    /// Реализация шага отправки: выводит цветное сообщение в консоль.
    /// </summary>
    protected override void SendMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  [ConsoleHandler:{_name}] {message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Переопределение hook-метода: подтверждает успешную отправку.
    /// </summary>
    protected override void LogResult()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [ConsoleHandler:{_name}] Уведомление отправлено.");
        Console.ResetColor();
    }
}
