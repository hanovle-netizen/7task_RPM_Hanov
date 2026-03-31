using MonitoringSystem.Strategy;

namespace MonitoringSystem.TemplateMethod;

/// <summary>
/// Конкретный обработчик (ConcreteClass): имитация отправки email.
/// Демонстрирует лёгкость добавления нового канала доставки
/// без изменения базового класса или других обработчиков.
/// </summary>
public class EmailHandler(string emailAddress, IFormatStrategy strategy) : EventHandlerBase(strategy)
{
    private readonly string _emailAddress = emailAddress;

    protected override string FormatMessage(string type, object data) =>
        _formatStrategy.Format($"[{type}] {data}", DateTime.Now);

    /// <summary>
    /// Имитация отправки email: в реальной системе здесь был бы SMTP-клиент.
    /// </summary>
    protected override void SendMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"  [EmailHandler] Отправка на {_emailAddress}:");
        Console.WriteLine($"  Тема: Критическое событие мониторинга");
        Console.WriteLine($"  Тело: {message}");
        Console.ResetColor();
    }

    protected override void LogResult()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [EmailHandler] Email отправлен на {_emailAddress}.");
        Console.ResetColor();
    }
}
