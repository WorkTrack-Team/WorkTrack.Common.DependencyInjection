// <copyright file="TestInstaller.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkTrack.Common.DependencyInjection.Tests;

/// <summary>
/// Тестовый установщик для проверки базовой функциональности.
/// </summary>
internal sealed class TestInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Тестовая реализация
    }
}
