// <copyright file="IInstallerTypeFilter.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Фильтр для определения валидности типа установщика.
/// </summary>
public interface IInstallerTypeFilter
{
    /// <summary>
    /// Проверяет, является ли тип валидным установщиком.
    /// </summary>
    /// <param name="type">Тип для проверки.</param>
    /// <returns>True, если тип является валидным установщиком.</returns>
    bool IsValidInstaller(Type type);
}
