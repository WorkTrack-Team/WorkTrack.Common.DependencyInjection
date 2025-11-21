// <copyright file="InstallerOptions.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Опции для настройки процесса регистрации установщиков.
/// </summary>
public sealed class InstallerOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether логировать ошибки при создании установщиков.
    /// </summary>
    public bool LogErrors { get; set; } = true;

    /// <summary>
    /// Gets or sets callback для обработки ошибок при создании установщиков.
    /// </summary>
    public Action<Type, Exception>? OnError { get; set; }

    /// <summary>
    /// Gets or sets пространства имен для исключения из поиска установщиков.
    /// </summary>
    public IReadOnlyList<string>? ExcludedNamespaces { get; set; }

    /// <summary>
    /// Gets or sets префиксы имен типов для исключения.
    /// </summary>
    public IReadOnlyList<string>? ExcludedTypeNamePrefixes { get; set; }
}
