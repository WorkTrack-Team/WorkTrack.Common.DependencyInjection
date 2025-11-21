using Ardalis.GuardClauses;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Настраиваемый фильтр для типов установщиков.
/// </summary>
public sealed class ConfigurableInstallerTypeFilter : IInstallerTypeFilter
{
    private readonly InstallerOptions _options;
    private readonly DefaultInstallerTypeFilter _defaultFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableInstallerTypeFilter"/> class.
    /// </summary>
    /// <param name="options">Опции для фильтрации.</param>
    public ConfigurableInstallerTypeFilter(InstallerOptions options)
    {
        Guard.Against.Null(options);
        _options = options;
        _defaultFilter = new DefaultInstallerTypeFilter();
    }

    /// <inheritdoc />
    public bool IsValidInstaller(Type type)
    {
        Guard.Against.Null(type);
        if (!_defaultFilter.IsValidInstaller(type))
        {
            return false;
        }

        return !IsExcluded(type);
    }

    private bool IsExcluded(Type type) =>
        IsExcludedByNamespace(type) || IsExcludedByPrefix(type);

    private bool IsExcludedByNamespace(Type type)
    {
        if (_options.ExcludedNamespaces == null)
        {
            return false;
        }

        return CheckNamespace(type);
    }

    private bool CheckNamespace(Type type)
    {
        var typeNamespace = type.Namespace ?? string.Empty;
        return _options.ExcludedNamespaces!.Any(ns => typeNamespace.StartsWith(ns, StringComparison.Ordinal));
    }

    private bool IsExcludedByPrefix(Type type)
    {
        if (_options.ExcludedTypeNamePrefixes == null)
        {
            return false;
        }

        return CheckPrefix(type);
    }

    private bool CheckPrefix(Type type)
    {
        var typeName = type.Name;
        return _options.ExcludedTypeNamePrefixes!.Any(prefix => typeName.StartsWith(prefix, StringComparison.Ordinal));
    }
}
