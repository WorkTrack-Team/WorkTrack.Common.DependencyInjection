// <copyright file="ServiceCollectionExtensionsWithDependenciesTests.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace WorkTrack.Common.DependencyInjection.Tests;

/// <summary>
/// Unit-тесты для <see cref="ServiceCollectionExtensions"/> с кастомными зависимостями.
/// </summary>
public sealed class ServiceCollectionExtensionsWithDependenciesTests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCollectionExtensionsWithDependenciesTests"/> class.
    /// </summary>
    public ServiceCollectionExtensionsWithDependenciesTests()
    {
        TestInstallerWithTracking.Reset();
    }

    /// <summary>
    /// Проверяет, что метод принимает кастомный discovery.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithCustomDiscovery_UsesCustomDiscovery()
    {
        // Arrange
        TestInstallerWithTracking.Reset();
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();
        var discovery = Substitute.For<IInstallerDiscovery>();
        var strategy = Substitute.For<IInstallerCreationStrategy>();

        discovery.DiscoverInstallers(Arg.Any<System.Reflection.Assembly>(), Arg.Any<IInstallerTypeFilter>())
            .Returns(new[] { typeof(TestInstallerWithTracking) });
        strategy.TryCreate(typeof(TestInstallerWithTracking), Arg.Any<Action<Type, Exception>?>())
            .Returns(new TestInstallerWithTracking());

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstaller>(
            configuration,
            discovery,
            strategy);

        // Assert
        services.Should().NotBeNull();
        discovery.Received(1).DiscoverInstallers(Arg.Any<System.Reflection.Assembly>(), Arg.Any<IInstallerTypeFilter>());
    }

    /// <summary>
    /// Проверяет, что метод выбрасывает исключение при null discovery.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithNullDiscovery_ThrowsArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();
        IInstallerDiscovery? discovery = null;
        var strategy = Substitute.For<IInstallerCreationStrategy>();

        // Act & Assert
        var act = () => services.InstallServicesFromAssemblyContaining<TestInstaller>(
            configuration,
            discovery!,
            strategy);
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Проверяет, что метод выбрасывает исключение при null strategy.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithNullStrategy_ThrowsArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();
        var discovery = Substitute.For<IInstallerDiscovery>();
        IInstallerCreationStrategy? strategy = null;

        // Act & Assert
        var act = () => services.InstallServicesFromAssemblyContaining<TestInstaller>(
            configuration,
            discovery,
            strategy!);
        act.Should().Throw<ArgumentNullException>();
    }
}
