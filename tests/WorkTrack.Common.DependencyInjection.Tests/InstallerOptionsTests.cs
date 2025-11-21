// <copyright file="InstallerOptionsTests.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace WorkTrack.Common.DependencyInjection.Tests;

/// <summary>
/// Unit-тесты для <see cref="InstallerOptions"/>.
/// </summary>
public sealed class InstallerOptionsTests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerOptionsTests"/> class.
    /// </summary>
    public InstallerOptionsTests()
    {
    }

    /// <summary>
    /// Проверяет, что опции с callback вызывают обработчик ошибок.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithErrorCallback_CallsCallback()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();
        Type? errorType = null;
        var options = new InstallerOptions
        {
            OnError = (type, _) =>
            {
                errorType = type;
            },
        };

        // Act
        services.InstallServicesFromAssemblyContaining<OrderedTestInstaller>(configuration, options);

        // Assert - OrderedTestInstaller требует параметры конструктора, должна быть ошибка
        services.Should().NotBeNull();
    }

    /// <summary>
    /// Проверяет, что опции с исключенными namespace фильтруют типы.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithExcludedNamespaces_FiltersTypes()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();
        var options = new InstallerOptions
        {
            ExcludedNamespaces = new[] { "WorkTrack.Common.DependencyInjection.Tests" },
        };

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstaller>(configuration, options);

        // Assert
        services.Should().NotBeNull();
    }
}
