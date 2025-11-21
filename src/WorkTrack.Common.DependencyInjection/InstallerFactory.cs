using Ardalis.GuardClauses;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Фабрика установщиков с поддержкой стратегий создания.
/// </summary>
internal sealed class InstallerFactory
{
    private readonly IInstallerCreationStrategy _strategy;

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallerFactory"/> class.
    /// </summary>
    /// <param name="strategy">Стратегия создания экземпляров.</param>
    public InstallerFactory(IInstallerCreationStrategy strategy)
    {
        Guard.Against.Null(strategy);
        _strategy = strategy;
    }

    /// <summary>
    /// Создает экземпляры установщиков из типов.
    /// </summary>
    /// <param name="types">Типы установщиков для создания.</param>
    /// <param name="onError">Callback для обработки ошибок.</param>
    /// <returns>Перечисление созданных установщиков.</returns>
    public IEnumerable<IServiceInstaller> CreateInstallers(
        IEnumerable<Type> types,
        Action<Type, Exception>? onError = null)
    {
        Guard.Against.Null(types);
        return types.Select(type => _strategy.TryCreate(type, onError)).OfType<IServiceInstaller>();
    }
}
