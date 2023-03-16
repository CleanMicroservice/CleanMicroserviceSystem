using System.Collections.Concurrent;
using NuGet.Frameworks;
using static NuGet.Frameworks.FrameworkConstants;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Indexing;

public class FrameworkCompatibilityService : IFrameworkCompatibilityService
{
    private const string AnyFramework = "any";

    private static readonly Dictionary<string, NuGetFramework> KnownFrameworks;
    private static readonly IReadOnlyList<OneWayCompatibilityMappingEntry> CompatibilityMapping;
    private static readonly ConcurrentDictionary<NuGetFramework, IReadOnlyList<string>> CompatibleFrameworks;

    static FrameworkCompatibilityService()
    {
        var supportedFrameworks = new HashSet<string>
        {
            FrameworkIdentifiers.NetStandard,
            FrameworkIdentifiers.NetCoreApp,
            FrameworkIdentifiers.Net
        };

        CompatibilityMapping = DefaultFrameworkMappings.Instance.CompatibilityMappings.ToList();
        CompatibleFrameworks = new ConcurrentDictionary<NuGetFramework, IReadOnlyList<string>>();

        KnownFrameworks = typeof(CommonFrameworks)
            .GetFields()
            .Where(f => f.IsStatic)
            .Where(f => f.FieldType == typeof(NuGetFramework))
            .Select(f => (NuGetFramework)f.GetValue(null))
            .Where(f => supportedFrameworks.Contains(f.Framework))
            .ToDictionary(f => f.GetShortFolderName());

        KnownFrameworks["net472"] = new NuGetFramework(FrameworkIdentifiers.Net, new Version(4, 7, 2, 0));
        KnownFrameworks["net471"] = new NuGetFramework(FrameworkIdentifiers.Net, new Version(4, 7, 1, 0));
    }

    public IReadOnlyList<string> FindAllCompatibleFrameworks(string name)
    {
        return !KnownFrameworks.TryGetValue(name, out var framework)
            ? new List<string> { name, AnyFramework }
            : CompatibleFrameworks.GetOrAdd(framework, this.FindAllCompatibleFrameworks);
    }

    private IReadOnlyList<string> FindAllCompatibleFrameworks(NuGetFramework targetFramework)
    {
        var results = new HashSet<string> { AnyFramework };

        foreach (var mapping in CompatibilityMapping)
        {
            if (!mapping.TargetFrameworkRange.Satisfies(targetFramework))
                continue;

            foreach (var possibleFramework in KnownFrameworks.Values)
            {
                if (mapping.SupportedFrameworkRange.Satisfies(possibleFramework))
                    _ = results.Add(possibleFramework.GetShortFolderName());
            }
        }

        foreach (var possibleFramework in KnownFrameworks.Values)
        {
            if (possibleFramework.Framework == targetFramework.Framework &&
                possibleFramework.Version <= targetFramework.Version)
            {
                _ = results.Add(possibleFramework.GetShortFolderName());
            }
        }

        return results.ToList();
    }
}