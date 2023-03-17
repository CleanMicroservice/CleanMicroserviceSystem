using CleanMicroserviceSystem.Astra.Contract.NuGetPackages;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public interface ISearchResponseBuilder
{
    SearchResponse BuildSearch(IReadOnlyList<PackageRegistration> results);

    AutocompleteResponse BuildAutocomplete(IReadOnlyList<string> data);

    DependentsResponse BuildDependents(IReadOnlyList<PackageDependent> results);
}