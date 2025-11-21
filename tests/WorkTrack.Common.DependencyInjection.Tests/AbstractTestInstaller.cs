// <copyright file="AbstractTestInstaller.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkTrack.Common.DependencyInjection.Tests;

/// <summary>
/// Абстрактный тестовый установщик для проверки игнорирования абстрактных классов.
/// </summary>
internal abstract class AbstractTestInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public abstract void Install(IServiceCollection services, IConfiguration configuration);
}
