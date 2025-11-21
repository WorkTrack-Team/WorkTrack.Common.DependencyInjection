// <copyright file="AnotherTestInstallerWithTracking.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkTrack.Common.DependencyInjection.Tests;

/// <summary>
/// Еще один тестовый установщик с отслеживанием.
/// </summary>
internal sealed class AnotherTestInstallerWithTracking : IServiceInstaller
{
    /// <summary>
    /// Gets a value indicating whether the Install method was called.
    /// </summary>
    public static bool WasCalled { get; private set; }

    /// <summary>
    /// Сбрасывает состояние отслеживания.
    /// </summary>
    public static void Reset()
    {
        WasCalled = false;
    }

    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        WasCalled = true;
    }
}
