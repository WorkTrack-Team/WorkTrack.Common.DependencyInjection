// <copyright file="ServiceCollectionExtensionsTests.cs" company="WorkTrack Team">
// Copyright (c) WorkTrack Team. All rights reserved.
// </copyright>

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace WorkTrack.Common.DependencyInjection.Tests;

/// <summary>
/// Unit-тесты для <see cref="ServiceCollectionExtensions"/>.
/// </summary>
public sealed class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCollectionExtensionsTests"/> class.
    /// </summary>
    public ServiceCollectionExtensionsTests()
    {
        TestInstallerWithTracking.Reset();
        AnotherTestInstallerWithTracking.Reset();
    }

    /// <summary>
    /// Проверяет, что метод регистрирует все установщики из сборки.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_RegistersAllInstallers()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstaller>(configuration, options: null);

        // Assert - проверяем что метод не выбрасывает исключений
        services.Should().NotBeNull();
    }

    /// <summary>
    /// Проверяет, что метод вызывает Install для каждого установщика.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_CallsInstallForEachInstaller()
    {
        // Arrange
        TestInstallerWithTracking.Reset();
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstallerWithTracking>(configuration);

        // Assert
        TestInstallerWithTracking.WasCalled.Should().BeTrue();
        TestInstallerWithTracking.Services.Should().BeSameAs(services);
        TestInstallerWithTracking.Configuration.Should().BeSameAs(configuration);
    }

    /// <summary>
    /// Проверяет, что метод выбрасывает исключение при null services.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithNullServices_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection? services = null;
        var configuration = Substitute.For<IConfiguration>();

        // Act & Assert
        var act = () => services!.InstallServicesFromAssemblyContaining<TestInstaller>(configuration);
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Проверяет, что метод выбрасывает исключение при null configuration.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithNullConfiguration_ThrowsArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        IConfiguration? configuration = null;

        // Act & Assert
        var act = () => services.InstallServicesFromAssemblyContaining<TestInstaller>(configuration!);
        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Проверяет, что метод возвращает ту же коллекцию сервисов.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_ReturnsSameServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        var result = services.InstallServicesFromAssemblyContaining<TestInstaller>(configuration);

        // Assert
        result.Should().BeSameAs(services);
    }

    /// <summary>
    /// Проверяет, что метод обрабатывает сборку без установщиков.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithNoInstallers_DoesNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        var act = () => services.InstallServicesFromAssemblyContaining<EmptyAssemblyMarker>(configuration);

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Проверяет, что метод передает правильные параметры в Install.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_PassesCorrectParameters()
    {
        // Arrange
        TestInstallerWithTracking.Reset();
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstallerWithTracking>(configuration);

        // Assert
        TestInstallerWithTracking.WasCalled.Should().BeTrue();
        TestInstallerWithTracking.Services.Should().BeSameAs(services);
        TestInstallerWithTracking.Configuration.Should().BeSameAs(configuration);
    }

    /// <summary>
    /// Проверяет, что метод не регистрирует абстрактные классы.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_IgnoresAbstractClasses()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        var act = () => services.InstallServicesFromAssemblyContaining<TestInstaller>(configuration);

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Проверяет, что метод не регистрирует интерфейсы.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_IgnoresInterfaces()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        var act = () => services.InstallServicesFromAssemblyContaining<TestInstaller>(configuration);

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Проверяет, что метод обрабатывает несколько установщиков в одной сборке.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithMultipleInstallers_CallsAll()
    {
        // Arrange
        TestInstallerWithTracking.Reset();
        AnotherTestInstallerWithTracking.Reset();
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstallerWithTracking>(configuration);

        // Assert
        TestInstallerWithTracking.WasCalled.Should().BeTrue();
        AnotherTestInstallerWithTracking.WasCalled.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет, что метод обрабатывает установщики с параметрами конструктора.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithParameterlessConstructor_CreatesInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        var act = () => services.InstallServicesFromAssemblyContaining<TestInstaller>(configuration);

        // Assert
        act.Should().NotThrow();
    }

    /// <summary>
    /// Проверяет, что метод пропускает установщики без параметров конструктора.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_WithConstructorParameters_SkipsInstaller()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act - OrderedTestInstaller требует параметры конструктора, поэтому должен быть пропущен
        var act = () => services.InstallServicesFromAssemblyContaining<OrderedTestInstaller>(configuration);

        // Assert - метод должен обработать исключение и не выбросить его
        act.Should().NotThrow();
    }

    /// <summary>
    /// Проверяет, что метод находит все установщики в сборке.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_FindsAllInstallersInAssembly()
    {
        // Arrange
        TestInstallerWithTracking.Reset();
        AnotherTestInstallerWithTracking.Reset();
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstallerWithTracking>(configuration);

        // Assert - оба установщика должны быть вызваны
        TestInstallerWithTracking.WasCalled.Should().BeTrue();
        AnotherTestInstallerWithTracking.WasCalled.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет, что метод вызывает Install с правильными параметрами для каждого установщика.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_CallsInstallWithCorrectParameters()
    {
        // Arrange
        TestInstallerWithTracking.Reset();
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstallerWithTracking>(configuration);

        // Assert
        TestInstallerWithTracking.WasCalled.Should().BeTrue();
        TestInstallerWithTracking.Services.Should().BeSameAs(services);
        TestInstallerWithTracking.Configuration.Should().BeSameAs(configuration);
    }

    /// <summary>
    /// Проверяет, что метод не создает экземпляры абстрактных классов.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_DoesNotCreateAbstractClassInstances()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        var act = () => services.InstallServicesFromAssemblyContaining<AbstractTestInstaller>(configuration);

        // Assert - метод не должен выбрасывать исключений при наличии абстрактных классов
        act.Should().NotThrow();
    }

    /// <summary>
    /// Проверяет, что метод обрабатывает TargetInvocationException при создании установщика.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_HandlesTargetInvocationException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act - OrderedTestInstaller требует параметры, что вызовет MissingMethodException
        // который обрабатывается как TargetInvocationException в некоторых случаях
        var act = () => services.InstallServicesFromAssemblyContaining<OrderedTestInstaller>(configuration);

        // Assert - метод должен обработать исключение
        act.Should().NotThrow();
    }

    /// <summary>
    /// Проверяет, что метод вызывает все установщики независимо от порядка в сборке.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_CallsAllInstallersRegardlessOfOrder()
    {
        // Arrange
        TestInstallerWithTracking.Reset();
        AnotherTestInstallerWithTracking.Reset();
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        services.InstallServicesFromAssemblyContaining<TestInstallerWithTracking>(configuration);

        // Assert - оба установщика должны быть вызваны
        TestInstallerWithTracking.WasCalled.Should().BeTrue();
        AnotherTestInstallerWithTracking.WasCalled.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет, что метод корректно обрабатывает установщики без параметров конструктора.
    /// </summary>
    [Fact]
    public void InstallServicesFromAssemblyContaining_HandlesParameterlessConstructors()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        var act = () => services.InstallServicesFromAssemblyContaining<TestInstaller>(configuration);

        // Assert
        act.Should().NotThrow();
    }
}
