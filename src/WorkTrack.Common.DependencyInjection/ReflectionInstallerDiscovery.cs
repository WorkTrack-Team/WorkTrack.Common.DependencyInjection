// <copyright file="ReflectionInstallerDiscovery.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using System.Reflection;
using Ardalis.GuardClauses;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Обнаружение установщиков через рефлексию.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ReflectionInstallerDiscovery"/> class.
/// </remarks>
/// <param name="filter">Фильтр для типов установщиков.</param>
internal sealed class ReflectionInstallerDiscovery(IInstallerTypeFilter? filter = null) : IInstallerDiscovery
{
    private readonly IInstallerTypeFilter _defaultFilter = filter ?? new DefaultInstallerTypeFilter();

    /// <inheritdoc />
    public IEnumerable<Type> DiscoverInstallers(Assembly assembly, IInstallerTypeFilter? filter = null)
    {
        Guard.Against.Null(assembly);
        var typeFilter = filter ?? _defaultFilter;
        return assembly.GetTypes().Where(typeFilter.IsValidInstaller);
    }
}
