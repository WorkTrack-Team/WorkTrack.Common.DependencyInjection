using Ardalis.GuardClauses;

namespace WorkTrack.Common.DependencyInjection;

/// <summary>
/// Настраиваемый фильтр для типов установщиков.
/// </summary>
public sealed class ConfigurableInstallerTypeFilter : IInstallerTypeFilter
{
    private readonly InstallerOptions options;
    private readonly DefaultInstallerTypeFilter defaultFilter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurableInstallerTypeFilter"/> class.
    /// </summary>
    /// <param name="options">Опции для фильтрации.</param>
    public ConfigurableInstallerTypeFilter(InstallerOptions options)
    {
        Guard.Against.Null(options);
        this.options = options;
        this.defaultFilter = new DefaultInstallerTypeFilter();
    }

    /// <inheritdoc />
    public bool IsValidInstaller(Type type)
    {
        Guard.Against.Null(type);
        if (!this.defaultFilter.IsValidInstaller(type))
        {
            return false;
        }

        return !this.IsExcluded(type);
    }

    private bool IsExcluded(Type type) =>
        this.IsExcludedByNamespace(type) || this.IsExcludedByPrefix(type);

    private bool IsExcludedByNamespace(Type type)
    {
        if (this.options.ExcludedNamespaces == null)
        {
            return false;
        }

        return this.CheckNamespace(type);
    }

    private bool CheckNamespace(Type type)
    {
        var typeNamespace = type.Namespace ?? string.Empty;
        return this.options.ExcludedNamespaces!.Any(ns => typeNamespace.StartsWith(ns, StringComparison.Ordinal));
    }

    private bool IsExcludedByPrefix(Type type)
    {
        if (this.options.ExcludedTypeNamePrefixes == null)
        {
            return false;
        }

        return this.CheckPrefix(type);
    }

    private bool CheckPrefix(Type type)
    {
        var typeName = type.Name;
        return this.options.ExcludedTypeNamePrefixes!.Any(prefix => typeName.StartsWith(prefix, StringComparison.Ordinal));
    }
}
