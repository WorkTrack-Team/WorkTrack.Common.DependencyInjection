// <copyright file="ActivatorCreationStrategy.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using System.Reflection;
using Ardalis.GuardClauses;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Стратегия создания через Activator.
/// </summary>
internal sealed class ActivatorCreationStrategy : IInstallerCreationStrategy
{
    /// <inheritdoc />
    public IServiceInstaller? TryCreate(Type type, Action<Type, Exception>? onError = null)
    {
        Guard.Against.Null(type);
        return CreateInstance(type, onError);
    }

    /// <summary>
    /// Создает экземпляр через Activator.
    /// </summary>
    private static IServiceInstaller? CreateInstance(Type type, Action<Type, Exception>? onError)
    {
        try
        {
            return CreateInstanceInternal(type);
        }
        catch (Exception ex) when (IsExpectedException(ex))
        {
            onError?.Invoke(type, ex);
            return null;
        }
    }

    private static bool IsExpectedException(Exception exception) =>
        exception is MissingMethodException or TargetInvocationException;

    private static IServiceInstaller? CreateInstanceInternal(Type type) =>
        (IServiceInstaller?)Activator.CreateInstance(type);
}
