using System.Collections;

namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;

public class PaginatedEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> values;
    public readonly int StartOfPage;
    public readonly int CountPerPage;
    public readonly int OriginCount;
    public readonly int CurrentPageIndex;
    public readonly int TotalPageCounts;

    public PaginatedEnumerable(
        IEnumerable<T> values,
        int startOfPage,
        int countPerPage,
        int originCount)
    {
        this.values = values;
        this.StartOfPage = startOfPage;
        this.CountPerPage = countPerPage;
        this.OriginCount = originCount;

        this.CurrentPageIndex = this.StartOfPage / this.CountPerPage;
        this.TotalPageCounts = (int)Math.Ceiling((double)OriginCount / this.CountPerPage);
    }

    public IEnumerator<T> GetEnumerator() => values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
