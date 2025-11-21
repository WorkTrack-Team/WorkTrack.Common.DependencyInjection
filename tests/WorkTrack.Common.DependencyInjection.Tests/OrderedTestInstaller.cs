// <copyright file="OrderedTestInstaller.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkTrack.Common.DependencyInjection.Tests;

/// <summary>
/// Тестовый установщик для проверки порядка вызовов.
/// </summary>
internal sealed class OrderedTestInstaller : IServiceInstaller
{
    private readonly string name;
    private readonly List<string> callOrder;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderedTestInstaller"/> class.
    /// </summary>
    /// <param name="name">Имя установщика.</param>
    /// <param name="callOrder">Список для отслеживания порядка вызовов.</param>
    public OrderedTestInstaller(string name, List<string> callOrder)
    {
        this.name = name;
        this.callOrder = callOrder;
    }

    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        this.callOrder.Add(this.name);
    }
}
