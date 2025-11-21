// <copyright file="ReflectionInstallerDiscovery.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using System.Reflection;
using Ardalis.GuardClauses;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Обнаружение установщиков через рефлексию.
/// </summary>
internal sealed class ReflectionInstallerDiscovery : IInstallerDiscovery
{
    private readonly IInstallerTypeFilter defaultFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReflectionInstallerDiscovery"/> class.
    /// </summary>
    /// <param name="filter">Фильтр для типов установщиков.</param>
    public ReflectionInstallerDiscovery(IInstallerTypeFilter? filter = null)
    {
        this.defaultFilter = filter ?? new DefaultInstallerTypeFilter();
    }

    /// <inheritdoc />
    public IEnumerable<Type> DiscoverInstallers(Assembly assembly, IInstallerTypeFilter? filter = null)
    {
        Guard.Against.Null(assembly);
        var typeFilter = filter ?? this.defaultFilter;
        return assembly.GetTypes().Where(typeFilter.IsValidInstaller);
    }
}
