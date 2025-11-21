// <copyright file="IServiceInstaller.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Определяет контракт для модульной регистрации сервисов.
/// </summary>
public interface IServiceInstaller
{
    /// <summary>
    /// Регистрирует зависимости модуля в контейнере сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    void Install(IServiceCollection services, IConfiguration configuration);
}
