// <copyright file="IInstallerCreationStrategy.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Стратегия создания экземпляров установщиков.
/// </summary>
public interface IInstallerCreationStrategy
{
    /// <summary>
    /// Пытается создать экземпляр установщика.
    /// </summary>
    /// <param name="type">Тип установщика для создания.</param>
    /// <param name="onError">Callback для обработки ошибок.</param>
    /// <returns>Экземпляр установщика или null, если создание невозможно.</returns>
    IServiceInstaller? TryCreate(Type type, Action<Type, Exception>? onError = null);
}
