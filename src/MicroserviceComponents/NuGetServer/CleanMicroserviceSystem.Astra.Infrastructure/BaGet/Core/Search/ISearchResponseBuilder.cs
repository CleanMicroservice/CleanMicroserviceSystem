using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Metadata;
using CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Search;

public interface ISearchResponseBuilder
{
    SearchResponse BuildSearch(IReadOnlyList<PackageRegistration> results);

    AutocompleteResponse BuildAutocomplete(IReadOnlyList<string> data);

    DependentsResponse BuildDependents(IReadOnlyList<PackageDependent> results);
}