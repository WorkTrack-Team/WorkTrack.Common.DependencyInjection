# WorkTrack.Common.DependencyInjection

Библиотека для модульной регистрации сервисов через паттерн `IServiceInstaller` в сервисах WorkTrack.

**Версия**: 1.0.0  
**Статус**: ✅ Готов к использованию

## Описание

Библиотека предоставляет паттерн `IServiceInstaller` для модульной регистрации зависимостей в DI-контейнере. Это позволяет организовать регистрацию сервисов по слоям и модулям, избегая дублирования кода между сервисами и обеспечивая единообразный подход к настройке DI во всех сервисах WorkTrack.

## Основные компоненты

- **`IServiceInstaller`** — интерфейс для установщиков сервисов
- **`ServiceCollectionExtensions.InstallServicesFromAssemblyContaining<TMarker>()`** — автоматическая регистрация всех установщиков из сборки
- **`InstallerOptions`** — опции для настройки процесса регистрации
- **`IInstallerDiscovery`** — интерфейс для кастомного обнаружения установщиков
- **`IInstallerCreationStrategy`** — интерфейс для кастомной стратегии создания установщиков

## Установка

### Через NuGet

```bash
dotnet add package WorkTrack.Common.DependencyInjection
```

### Через .csproj

```xml
<ItemGroup>
  <PackageReference Include="WorkTrack.Common.DependencyInjection" Version="1.0.0" />
</ItemGroup>
```

## Быстрый старт

### 1. Создание простого установщика

Создайте класс, реализующий `IServiceInstaller`:

```csharp
using WorkTrack.Common.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ardalis.GuardClauses;

namespace MyService.Infrastructure.DependencyInjection;

/// <summary>
/// Регистрация сервисов доступа к данным.
/// </summary>
public sealed class DataInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(configuration);
        
        // Регистрация DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(connectionString));
        
        // Регистрация репозиториев
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
    }
}
```

### 2. Регистрация в Program.cs

Используйте метод расширения для автоматической регистрации всех установщиков:

```csharp
using WorkTrack.Common.DependencyInjection;
using MyService.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Автоматически найдет и зарегистрирует все классы, 
// реализующие IServiceInstaller в сборке, содержащей DataInstaller
builder.Services.InstallServicesFromAssemblyContaining<DataInstaller>(
    configuration: builder.Configuration);

var app = builder.Build();
app.Run();
```

## Подробное использование

### Оркестрация установщиков

Для больших проектов рекомендуется создать главный установщик, который оркестрирует другие:

```csharp
namespace MyService.Infrastructure.DependencyInjection;

/// <summary>
/// Регистрация инфраструктурных зависимостей.
/// Оркестрирует установку всех модулей инфраструктуры.
/// </summary>
public sealed class InfrastructureInstaller : IServiceInstaller
{
    private readonly IServiceInstaller[] _installers =
    [
        new CoreInstaller(),
        new DataInstaller(),
        new MessagingInstaller(),
        new AuthenticationInstaller(),
    ];

    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(configuration);

        foreach (var installer in _installers)
        {
            installer.Install(services, configuration);
        }
    }
}
```

Затем в `Program.cs`:

```csharp
builder.Services.InstallServicesFromAssemblyContaining<InfrastructureInstaller>(
    configuration: builder.Configuration);
```

### Использование InstallerOptions

Для настройки процесса регистрации используйте `InstallerOptions`:

```csharp
var options = new InstallerOptions
{
    // Отключить логирование ошибок
    LogErrors = false,
    
    // Callback для обработки ошибок при создании установщиков
    OnError = (type, exception) => 
    {
        logger.LogError(exception, "Не удалось создать установщик {Type}", type.Name);
    },
    
    // Исключить определенные пространства имен из поиска
    ExcludedNamespaces = new[] { "MyService.Tests", "MyService.Migrations" },
    
    // Исключить типы с определенными префиксами
    ExcludedTypeNamePrefixes = new[] { "Test", "Mock", "Fake" }
};

builder.Services.InstallServicesFromAssemblyContaining<InfrastructureInstaller>(
    configuration: builder.Configuration,
    options: options);
```

### Продвинутое использование с кастомными зависимостями

Для полного контроля над процессом обнаружения и создания установщиков можно использовать перегрузку с кастомными `IInstallerDiscovery` и `IInstallerCreationStrategy`:

```csharp
using WorkTrack.Common.DependencyInjection;

// Создание кастомного discovery
var customDiscovery = new ReflectionInstallerDiscovery();

// Создание кастомной стратегии создания
var customStrategy = new ActivatorCreationStrategy();

builder.Services.InstallServicesFromAssemblyContaining<InfrastructureInstaller>(
    configuration: builder.Configuration,
    discovery: customDiscovery,
    strategy: customStrategy);
```

Это полезно для:
- Кастомной фильтрации типов установщиков
- Специальной логики создания экземпляров
- Тестирования с моками

## Примеры из реальных проектов

### WorkTrack.Identity

В проекте `WorkTrack.Identity` используется следующая структура:

```csharp
// InfrastructureInstaller.cs
public sealed class InfrastructureInstaller : IServiceInstaller
{
    private readonly IServiceInstaller[] _installers =
    [
        new CoreInstaller(),
        new DataInstaller(),
        new AuthorizationInstaller(),
        new KafkaInstaller(),
        new KeycloakInstaller(),
        new TemplatesInstaller(),
    ];

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(configuration);

        foreach (var installer in _installers)
        {
            installer.Install(services, configuration);
        }
    }
}
```

```csharp
// DataInstaller.cs
public sealed class DataInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(configuration);

        AddDbContext(services, configuration);
        RegisterRepositories(services);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        Guard.Against.NullOrWhiteSpace(connectionString);
        services.AddApplicationDbContext(connectionString);
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
    }
}
```

## Best Practices

### 1. Один установщик на модуль/слой

Создавайте отдельный установщик для каждого логического модуля:

```csharp
public sealed class DataInstaller : IServiceInstaller { }      // Data access
public sealed class MessagingInstaller : IServiceInstaller { } // Messaging
public sealed class AuthInstaller : IServiceInstaller { }      // Authentication
```

### 2. Используйте Guard.Against для валидации

Всегда проверяйте входные параметры:

```csharp
public void Install(IServiceCollection services, IConfiguration configuration)
{
    Guard.Against.Null(services);
    Guard.Against.Null(configuration);
    // ...
}
```

### 3. Разбивайте большие установщики на методы

Для улучшения читаемости выносите логику в отдельные методы:

```csharp
public void Install(IServiceCollection services, IConfiguration configuration)
{
    Guard.Against.Null(services);
    Guard.Against.Null(configuration);
    
    AddDbContext(services, configuration);
    RegisterRepositories(services);
    RegisterInterceptors(services);
}

private static void AddDbContext(IServiceCollection services, IConfiguration configuration) { }
private static void RegisterRepositories(IServiceCollection services) { }
private static void RegisterInterceptors(IServiceCollection services) { }
```

### 4. Используйте sealed классы

Установщики должны быть `sealed` для предотвращения наследования:

```csharp
public sealed class DataInstaller : IServiceInstaller
{
    // ...
}
```

### 5. Избегайте установщиков с параметрами в конструкторе

Установщики должны иметь параметрless конструктор. Если нужны зависимости, получайте их через `IConfiguration` или регистрируйте через другой установщик.

### 6. Оркестрируйте установщики через главный установщик

Для больших проектов создавайте главный установщик, который вызывает другие:

```csharp
public sealed class InfrastructureInstaller : IServiceInstaller
{
    private readonly IServiceInstaller[] _installers = [ /* ... */ ];
    
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        foreach (var installer in _installers)
        {
            installer.Install(services, configuration);
        }
    }
}
```

## Архитектура

Библиотека следует принципам SOLID и использует паттерны проектирования:

- **Strategy Pattern** — `IInstallerDiscovery`, `IInstallerCreationStrategy` для гибкости
- **Factory Pattern** — `InstallerFactory` для создания установщиков
- **Dependency Inversion Principle** — зависимости через интерфейсы
- **Single Responsibility Principle** — каждый класс отвечает за одну задачу

### Основные компоненты

```
IServiceInstaller
    ↓
ServiceCollectionExtensions
    ↓
InstallerRegistry (оркестрация)
    ↓
    ├── IInstallerDiscovery (обнаружение типов)
    └── InstallerFactory
        └── IInstallerCreationStrategy (создание экземпляров)
```

## Преимущества

- ✅ **Единообразие** — один паттерн для всех сервисов WorkTrack
- ✅ **Модульность** — каждый слой/модуль регистрирует свои сервисы
- ✅ **Тестируемость** — легко мокировать установщики в тестах
- ✅ **Читаемость** — явная структура регистрации сервисов
- ✅ **Переиспользование** — не нужно дублировать код в каждом сервисе
- ✅ **Гибкость** — поддержка кастомных стратегий обнаружения и создания
- ✅ **Надежность** — graceful handling ошибок при создании установщиков
- ✅ **Конфигурируемость** — через `InstallerOptions` можно настроить поведение

## Обработка ошибок

Библиотека gracefully обрабатывает следующие ситуации:

- Установщики с параметрами в конструкторе — пропускаются
- Абстрактные классы и интерфейсы — игнорируются
- Ошибки при создании экземпляров — логируются через callback
- Пустые сборки — не вызывают исключений

## Применение в сервисах

Все сервисы WorkTrack используют этот паттерн:

- `WorkTrack.Identity` — через `WorkTrack.Identity.Shared`
- `WorkTrack.TemplateService` — через `WorkTrack.TemplateService.Shared`
- Будущие сервисы — через NuGet пакет `WorkTrack.Common.DependencyInjection`

## Зависимости

- `Microsoft.Extensions.DependencyInjection.Abstractions` (>= 9.0.0)
- `Microsoft.Extensions.Configuration.Abstractions` (>= 9.0.0)
- `Ardalis.GuardClauses` (>= 5.0.0)
- `WorkTrack.Common.Analyzers` (>= 1.0.3) — для анализаторов кода

## Лицензия

Copyright (c) WorkTrack Team. All rights reserved.
