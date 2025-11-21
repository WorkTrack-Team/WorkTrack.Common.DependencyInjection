// <copyright file="IInstallerDiscovery.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using System.Reflection;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Обнаружение установщиков в сборке.
/// </summary>
public interface IInstallerDiscovery
{
    /// <summary>
    /// Находит все типы установщиков в сборке.
    /// </summary>
    /// <param name="assembly">Сборка для поиска установщиков.</param>
    /// <param name="filter">Опциональный фильтр для типов.</param>
    /// <returns>Перечисление типов установщиков.</returns>
    IEnumerable<Type> DiscoverInstallers(Assembly assembly, IInstallerTypeFilter? filter = null);
}
