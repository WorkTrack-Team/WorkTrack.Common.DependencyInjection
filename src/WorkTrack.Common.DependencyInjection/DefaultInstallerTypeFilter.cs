// <copyright file="DefaultInstallerTypeFilter.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Фильтр по умолчанию для типов установщиков.
/// </summary>
public sealed class DefaultInstallerTypeFilter : IInstallerTypeFilter
{
    /// <inheritdoc />
    public bool IsValidInstaller(Type type) =>
        type is { IsAbstract: false, IsInterface: false }
        && typeof(IServiceInstaller).IsAssignableFrom(type);
}
