// <copyright file="TestInstallerWithTracking.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkTrack.Common.DependencyInjection.Tests;

/// <summary>
/// Тестовый установщик с отслеживанием вызовов.
/// </summary>
internal sealed class TestInstallerWithTracking : IServiceInstaller
{
    /// <summary>
    /// Gets a value indicating whether the Install method was called.
    /// </summary>
    public static bool WasCalled { get; private set; }

    /// <summary>
    /// Gets the service collection that was passed to Install.
    /// </summary>
    public static IServiceCollection? Services { get; private set; }

    /// <summary>
    /// Gets the configuration that was passed to Install.
    /// </summary>
    public static IConfiguration? Configuration { get; private set; }

    /// <summary>
    /// Сбрасывает состояние отслеживания.
    /// </summary>
    public static void Reset()
    {
        WasCalled = false;
        Services = null;
        Configuration = null;
    }

    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        WasCalled = true;
        Services = services;
        Configuration = configuration;
    }
}
