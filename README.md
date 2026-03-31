# Лабораторная работа №7
## Поведенческие паттерны проектирования: Наблюдатель, Стратегия, Шаблонный метод

---

## 1. Тема и цель работы

**Тема:** Применение поведенческих паттернов проектирования (Наблюдатель, Стратегия, Шаблонный метод) при разработке системы мониторинга и оповещения о событиях.

**Цель:** Формирование практических навыков применения поведенческих паттернов проектирования в рамках единой программной системы.

---

## 2. Описание реализованной системы

Реализована имитация системы мониторинга, которая:
- Отслеживает метрики: загрузка CPU, использование памяти, сетевой трафик
- Генерирует события при превышении пороговых значений
- Уведомляет подписчиков через разные каналы: консоль, файл, email (имитация)
- Форматирует сообщения в трёх форматах: Text, JSON, HTML

---

## 3. Архитектура и структура проекта

```
MonitoringSystem/
├── Observer/
│   ├── MetricData.cs          # Данные события (передаются издателем)
│   ├── MetricEventArgs.cs     # Аргументы события (EventArgs)
│   └── EventMonitor.cs        # Субъект/Издатель (Subject)
├── Strategy/
│   ├── IFormatStrategy.cs     # Интерфейс стратегии форматирования
│   ├── TextFormatStrategy.cs  # Конкретная стратегия: текст
│   ├── JsonFormatStrategy.cs  # Конкретная стратегия: JSON
│   └── HtmlFormatStrategy.cs  # Конкретная стратегия: HTML
├── TemplateMethod/
│   ├── EventHandlerBase.cs    # Абстрактный класс (шаблонный метод + контекст стратегии)
│   ├── ConsoleHandler.cs      # Конкретный класс: вывод в консоль
│   ├── FileHandler.cs         # Конкретный класс: запись в файл
│   └── EmailHandler.cs        # Конкретный класс: отправка email (имитация)
└── Program.cs                 # Точка входа, демонстрация работы
```

---

## 4. Реализация паттернов

### 4.1. Паттерн «Наблюдатель» (Observer)

Реализован через встроенные механизмы C#: **делегаты** и **события** (`event`).

- **Издатель (Subject):** `EventMonitor` — содержит событие `OnMetricExceeded` типа `MetricEventHandler`
- **Подписчики (Observers):** `ConsoleHandler`, `FileHandler`, `EmailHandler` — подписываются оператором `+=`
- **Данные события:** `MetricEventArgs` содержит тип события и объект `MetricData`

```csharp
// Делегат определяет сигнатуру обработчиков
public delegate void MetricEventHandler(MetricEventArgs e);

// Издатель публикует событие
public class EventMonitor
{
    public event MetricEventHandler? OnMetricExceeded;

    public void CheckMetric(string metricName, double value, double threshold)
    {
        if (value > threshold)
        {
            var eventData = new MetricData(metricName, value, threshold, DateTime.Now);
            OnMetricExceeded?.Invoke(new MetricEventArgs(metricName + "_Exceeded", eventData));
        }
    }
}
```

**Почему `event` вместо классического Observer?**
Конструкция `event` в C# автоматически делает снимок списка подписчиков перед вызовом, что защищает от состояния гонки (Race Condition) при многопоточном использовании.

---

### 4.2. Паттерн «Стратегия» (Strategy)

Отвечает за **формат сообщений**. Позволяет менять формат без изменения обработчиков.

- **Интерфейс:** `IFormatStrategy` с методом `Format(string message, DateTime timestamp)`
- **Конкретные стратегии:** `TextFormatStrategy`, `JsonFormatStrategy`, `HtmlFormatStrategy`
- **Контекст:** `EventHandlerBase` хранит текущую стратегию и предоставляет метод `SetStrategy()`

```csharp
public interface IFormatStrategy
{
    string Format(string message, DateTime timestamp);
}

// Пример: смена стратегии во время выполнения
consoleHandler.SetStrategy(new HtmlFormatStrategy());
```

---

### 4.3. Паттерн «Шаблонный метод» (Template Method)

Определяет **скелет алгоритма обработки события** в базовом классе `EventHandlerBase`.

Шаблонный метод `ProcessEvent` фиксирует три шага:
1. `FormatMessage` — форматирование (абстрактный, реализуют подклассы)
2. `SendMessage` — отправка в канал доставки (абстрактный)
3. `LogResult` — логирование результата (hook-метод, виртуальный)

```csharp
// Скелет алгоритма — зафиксирован в базовом классе
public void ProcessEvent(MetricEventArgs e)
{
    var message = FormatMessage(e.EventType, e.Data);  // шаг 1
    SendMessage(message);                               // шаг 2
    LogResult();                                        // шаг 3 (hook)
}

// Подкласс реализует только детали
protected override void SendMessage(string message)
{
    Console.WriteLine(message);  // ConsoleHandler
}
```

---

## 5. Взаимодействие паттернов

```
Поток данных при наступлении события:

EventMonitor.CheckMetric("CPU_Load", 95.0, 80.0)
    │
    ├─► Создаёт MetricData + MetricEventArgs
    │
    └─► OnMetricExceeded?.Invoke(args)          ← НАБЛЮДАТЕЛЬ
            │
            ├─► ConsoleHandler.ProcessEvent(args)
            │       │
            │       ├─► FormatMessage()          ← ШАБЛОННЫЙ МЕТОД
            │       │       └─► _formatStrategy.Format()  ← СТРАТЕГИЯ
            │       ├─► SendMessage()            ← ШАБЛОННЫЙ МЕТОД
            │       └─► LogResult()              ← hook-метод
            │
            ├─► FileHandler.ProcessEvent(args)
            │       └─► (тот же алгоритм, другой канал)
            │
            └─► EmailHandler.ProcessEvent(args)
                    └─► (тот же алгоритм, другой канал)
```

---

## 6. Демонстрация работы

### Запуск

```bash
cd MonitoringSystem
dotnet run
```

### Вывод программы

Программа демонстрирует:
1. Нормальные значения метрик — события не генерируются
2. Превышение порога CPU (95 > 80) — все 4 подписчика получают уведомление
3. Превышение порога памяти (85 > 75) — уведомления во всех форматах
4. Превышение сетевого трафика (750 > 500)
5. Смена стратегии форматирования в runtime (JSON → HTML)
6. Отписку EmailHandler от событий

HTML-уведомления записываются в файл `alerts.html`.

---

## 7. Ответы на контрольные вопросы

**1. Паттерн для доставки уведомлений?**
Паттерн «Наблюдатель» (Observer). Он оптимален, так как позволяет `EventMonitor` уведомлять произвольное число обработчиков, не зная их конкретных типов — только через делегат `MetricEventHandler`.

**2. Как «Стратегия» обеспечивает гибкость форматирования?**
`EventHandlerBase` хранит ссылку на `IFormatStrategy` и делегирует форматирование стратегии. Благодаря методу `SetStrategy()` формат можно менять прямо во время выполнения — без пересоздания обработчика.

**3. Роль шаблонного метода в `EventHandlerBase`?**
`ProcessEvent` — это шаблонный метод. Он фиксирует порядок шагов: форматирование → отправка → логирование. Фиксированный шаг — сама последовательность. Переопределяемые шаги — `FormatMessage`, `SendMessage` (обязательно) и `LogResult` (hook, опционально).

**4. Взаимодействие трёх паттернов?**
Observer доставляет событие от `EventMonitor` к обработчикам. Template Method определяет алгоритм в обработчике. Strategy форматирует сообщение внутри этого алгоритма. Все три работают в цепочке при каждом событии.

**5. Принципы SOLID:**
- **SRP:** каждый класс отвечает за одно — `EventMonitor` только мониторит, `ConsoleHandler` только выводит в консоль
- **OCP:** добавление нового формата (`XmlFormatStrategy`) или канала (`SmsHandler`) не требует изменения существующих классов
- **LSP:** все стратегии взаимозаменяемы через `IFormatStrategy`; все обработчики работают одинаково через `EventHandlerBase`
- **DIP:** `EventHandlerBase` зависит от абстракции `IFormatStrategy`, а не от конкретных классов

**6. Расширение системы:**
- Новый канал (SMS): создать `SmsHandler : EventHandlerBase`, реализовать `SendMessage` — всё остальное без изменений
- Новый формат (XML): создать `XmlFormatStrategy : IFormatStrategy` — 5 строк кода

**7. Стратегия vs Шаблонный метод:**
«Стратегия» использует **композицию** — алгоритм находится в отдельном объекте, который можно заменить в runtime. «Шаблонный метод» использует **наследование** — скелет зафиксирован в базовом классе, детали переопределяются в подклассе. Стратегия гибче; Шаблонный метод проще, когда структура стабильна.

**8. Проблемы Observer в многопоточной среде:**
- **Race Condition** при переборе коллекции: решается снимком коллекции (`ToArray()`) или конструкцией `event` (C# делает это автоматически)
- **Блокировка издателя**: медленный подписчик блокирует всех — решается вызовом через `Task.Run()`
- **Deadlock**: если подписчик пытается отписаться внутри `Update` — решается потокобезопасными коллекциями (`ConcurrentBag`)

---

## 8. Технологии

- **Язык:** C# 12, .NET 8
- **Тип проекта:** Console Application
- **IDE:** Visual Studio / Rider
