using MonitoringSystem.Strategy;

namespace MonitoringSystem.TemplateMethod;

/// <summary>
/// Конкретный обработчик (ConcreteClass): запись уведомлений в файл.
/// Реализует абстрактные шаги базового класса для канала «файл».
/// </summary>
public class FileHandler(string filePath, IFormatStrategy strategy) : EventHandlerBase(strategy)
{
    private readonly string _filePath = filePath;

    /// <summary>
    /// Реализация шага форматирования: строит текст и применяет стратегию.
    /// </summary>
    protected override string FormatMessage(string type, object data) =>
        _formatStrategy.Format($"[{type}] {data}", DateTime.Now);

    /// <summary>
    /// Реализация шага отправки: дописывает сообщение в конец файла.
    /// </summary>
    protected override void SendMessage(string message)
    {
        File.AppendAllText(_filePath, message + Environment.NewLine);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  [FileHandler] Запись в файл: {_filePath}");
        Console.ResetColor();
    }

    /// <summary>
    /// Переопределение hook-метода: сообщает о завершении записи.
    /// </summary>
    protected override void LogResult()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [FileHandler] Запись добавлена в {_filePath}.");
        Console.ResetColor();
    }
}
