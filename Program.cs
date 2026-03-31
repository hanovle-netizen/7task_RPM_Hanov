using MonitoringSystem.Observer;
using MonitoringSystem.Strategy;
using MonitoringSystem.TemplateMethod;

// ============================================================
//  ДЕМОНСТРАЦИЯ СИСТЕМЫ МОНИТОРИНГА И ОПОВЕЩЕНИЯ
//  Паттерны: Наблюдатель + Стратегия + Шаблонный метод
// ============================================================

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("╔══════════════════════════════════════════════════════╗");
Console.WriteLine("║    СИСТЕМА МОНИТОРИНГА И ОПОВЕЩЕНИЯ О СОБЫТИЯХ      ║");
Console.WriteLine("║  Observer + Strategy + Template Method               ║");
Console.WriteLine("╚══════════════════════════════════════════════════════╝");
Console.ResetColor();
Console.WriteLine();

// ----------------------------------------------------------
// 1. ПАТТЕРН «НАБЛЮДАТЕЛЬ» — создаём издателя (Subject)
// ----------------------------------------------------------
var monitor = new EventMonitor();

// ----------------------------------------------------------
// 2. ПАТТЕРН «ШАБЛОННЫЙ МЕТОД» + «СТРАТЕГИЯ»
//    Создаём обработчики (ConcreteClass) с разными стратегиями
// ----------------------------------------------------------

// ConsoleHandler с текстовым форматом
var consoleText = new ConsoleHandler("TEXT", new TextFormatStrategy());

// ConsoleHandler с JSON форматом
var consoleJson = new ConsoleHandler("JSON", new JsonFormatStrategy());

// FileHandler с HTML форматом
var fileHtml = new FileHandler("alerts.html", new HtmlFormatStrategy());

// EmailHandler с текстовым форматом (имитация)
var emailText = new EmailHandler("admin@company.com", new TextFormatStrategy());

// ----------------------------------------------------------
// 3. ПАТТЕРН «НАБЛЮДАТЕЛЬ» — подписка (Attach)
//    Метод ProcessEvent имеет подходящую сигнатуру для делегата
// ----------------------------------------------------------
monitor.OnMetricExceeded += consoleText.ProcessEvent;
monitor.OnMetricExceeded += consoleJson.ProcessEvent;
monitor.OnMetricExceeded += fileHtml.ProcessEvent;
monitor.OnMetricExceeded += emailText.ProcessEvent;

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Подписчики зарегистрированы: ConsoleText, ConsoleJSON, FileHTML, EmailText");
Console.ResetColor();
Console.WriteLine();

// ----------------------------------------------------------
// 4. ИМИТАЦИЯ МОНИТОРИНГА МЕТРИК
// ----------------------------------------------------------

// --- CPU ---
PrintSection("Мониторинг загрузки процессора (CPU)");
monitor.CheckMetric("CPU_Load", 45.0, 80.0);   // Норма — событие НЕ генерируется
Console.WriteLine();
monitor.CheckMetric("CPU_Load", 95.0, 80.0);   // Превышение — событие генерируется
Console.WriteLine();

// --- Память ---
PrintSection("Мониторинг использования памяти (Memory)");
monitor.CheckMetric("Memory_Usage", 60.0, 75.0);  // Норма
Console.WriteLine();
monitor.CheckMetric("Memory_Usage", 85.0, 75.0);  // Превышение
Console.WriteLine();

// --- Сеть ---
PrintSection("Мониторинг сетевой активности (Network)");
monitor.CheckMetric("Network_Traffic", 200.0, 500.0);  // Норма
Console.WriteLine();
monitor.CheckMetric("Network_Traffic", 750.0, 500.0);  // Превышение
Console.WriteLine();

// ----------------------------------------------------------
// 5. ДЕМОНСТРАЦИЯ СМЕНЫ СТРАТЕГИИ В RUNTIME (паттерн «Стратегия»)
// ----------------------------------------------------------
PrintSection("Смена стратегии форматирования в runtime");
Console.WriteLine("Меняем стратегию ConsoleJSON с JSON на HTML...");
consoleJson.SetStrategy(new HtmlFormatStrategy());
monitor.CheckMetric("CPU_Load", 99.0, 80.0);
Console.WriteLine();

// ----------------------------------------------------------
// 6. ДЕМОНСТРАЦИЯ ОТПИСКИ (Detach) — паттерн «Наблюдатель»
// ----------------------------------------------------------
PrintSection("Отписка EmailHandler от событий");
monitor.OnMetricExceeded -= emailText.ProcessEvent;
Console.WriteLine("EmailHandler отписан. Теперь email-уведомления не отправляются.");
Console.WriteLine();
monitor.CheckMetric("Memory_Usage", 92.0, 75.0);
Console.WriteLine();

// ----------------------------------------------------------
// 7. ИТОГ
// ----------------------------------------------------------
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("╔══════════════════════════════════════════════════════╗");
Console.WriteLine("║                  ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА             ║");
Console.WriteLine("║  Результаты HTML записаны в файл: alerts.html       ║");
Console.WriteLine("╚══════════════════════════════════════════════════════╝");
Console.ResetColor();

Console.WriteLine("\nНажмите любую клавишу для выхода...");
if (Console.IsInputRedirected)
    Console.Read();
else
    Console.ReadKey();

// ----------------------------------------------------------
// Вспомогательный метод для заголовков секций
// ----------------------------------------------------------
static void PrintSection(string title)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"--- {title} ---");
    Console.ResetColor();
}
