// <copyright file="InstallerRegistry.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using System.Reflection;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Регистратор установщиков (оркестрация процесса регистрации).
/// </summary>
internal sealed class InstallerRegistry
{
    private readonly IInstallerDiscovery discovery;
    private readonly InstallerFactory factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerRegistry"/> class.
    /// </summary>
    /// <param name="discovery">Сервис обнаружения установщиков.</param>
    /// <param name="factory">Фабрика для создания установщиков.</param>
    public InstallerRegistry(IInstallerDiscovery discovery, InstallerFactory factory)
    {
        Guard.Against.Null(discovery);
        Guard.Against.Null(factory);
        this.discovery = discovery;
        this.factory = factory;
    }

    /// <summary>
    /// Регистрирует все установщики из сборки.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="assembly">Сборка для поиска установщиков.</param>
    /// <param name="options">Опции для настройки процесса регистрации.</param>
    public void RegisterInstallers(
        IServiceCollection services,
        IConfiguration configuration,
        Assembly assembly,
        InstallerOptions? options = null)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(configuration);
        Guard.Against.Null(assembly);
        var opts = options ?? new InstallerOptions();
        var installers = this.ResolveInstallers(assembly, opts);
        RegisterAll(installers, services, configuration);
    }

    private static void RegisterAll(
        IEnumerable<IServiceInstaller> installers,
        IServiceCollection services,
        IConfiguration configuration)
    {
        foreach (var installer in installers)
        {
            installer.Install(services, configuration);
        }
    }

    private static Action<Type, Exception>? CreateErrorHandler(InstallerOptions options) =>
        options.OnError ?? (options.LogErrors ? CreateDefaultErrorHandler() : null);

    private static Action<Type, Exception> CreateDefaultErrorHandler() =>
        (type, ex) => { /* Можно добавить логирование через ILogger если нужно */ };

    private static ConfigurableInstallerTypeFilter? CreateTypeFilter(InstallerOptions options)
    {
        if (options.ExcludedNamespaces == null && options.ExcludedTypeNamePrefixes == null)
        {
            return null;
        }

        return new ConfigurableInstallerTypeFilter(options);
    }

    private IEnumerable<IServiceInstaller> ResolveInstallers(Assembly assembly, InstallerOptions options)
    {
        var filter = CreateTypeFilter(options);
        var installerTypes = this.discovery.DiscoverInstallers(assembly, filter);
        var onError = CreateErrorHandler(options);
        return this.factory.CreateInstallers(installerTypes, onError);
    }
}
