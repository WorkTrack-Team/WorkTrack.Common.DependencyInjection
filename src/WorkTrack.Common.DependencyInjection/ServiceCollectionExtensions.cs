// <copyright file="ServiceCollectionExtensions.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Расширения для модульной регистрации сервисов.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует сервисы всех установщиков в сборке, содержащей указанный тип.
    /// </summary>
    /// <typeparam name="TMarker">Тип-маркер сборки.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="options">Опции для настройки процесса регистрации.</param>
    /// <returns>Коллекция сервисов с зарегистрированными установщиками.</returns>
    public static IServiceCollection InstallServicesFromAssemblyContaining<TMarker>(
        this IServiceCollection services,
        IConfiguration configuration,
        InstallerOptions? options = null)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(configuration);
        var registry = CreateRegistry();
        registry.RegisterInstallers(services, configuration, typeof(TMarker).Assembly, options);
        return services;
    }

    /// <summary>
    /// Регистрирует сервисы всех установщиков в сборке, содержащей указанный тип с кастомными зависимостями.
    /// </summary>
    /// <typeparam name="TMarker">Тип-маркер сборки.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="discovery">Сервис обнаружения установщиков.</param>
    /// <param name="strategy">Стратегия создания установщиков.</param>
    /// <param name="options">Опции для настройки процесса регистрации.</param>
    /// <returns>Коллекция сервисов с зарегистрированными установщиками.</returns>
    public static IServiceCollection InstallServicesFromAssemblyContaining<TMarker>(
        this IServiceCollection services,
        IConfiguration configuration,
        IInstallerDiscovery discovery,
        IInstallerCreationStrategy strategy,
        InstallerOptions? options = null)
    {
        Guard.Against.Null(services);
        Guard.Against.Null(configuration);
        Guard.Against.Null(discovery);
        Guard.Against.Null(strategy);
        var registry = CreateRegistry(discovery, strategy);
        return RegisterInstallers(registry, services, configuration, typeof(TMarker).Assembly, options);
    }

    private static IServiceCollection RegisterInstallers(
        InstallerRegistry registry,
        IServiceCollection services,
        IConfiguration configuration,
        System.Reflection.Assembly assembly,
        InstallerOptions? options)
    {
        registry.RegisterInstallers(services, configuration, assembly, options);
        return services;
    }

    /// <summary>
    /// Создает регистратор с дефолтными стратегиями.
    /// </summary>
    /// <param name="discovery">Опциональный сервис discovery.</param>
    /// <param name="strategy">Опциональная стратегия создания.</param>
    /// <returns>Регистратор установщиков с настройками по умолчанию.</returns>
    private static InstallerRegistry CreateRegistry(
        IInstallerDiscovery? discovery = null,
        IInstallerCreationStrategy? strategy = null)
    {
        var actualDiscovery = discovery ?? CreateDiscovery();
        var actualStrategy = strategy ?? CreateStrategy();
        var factory = new InstallerFactory(actualStrategy);
        return new InstallerRegistry(actualDiscovery, factory);
    }

    private static ReflectionInstallerDiscovery CreateDiscovery() => new();

    private static ActivatorCreationStrategy CreateStrategy() => new();
}
