using System.Collections;

namespace CleanMicroserviceSystem.DataStructure;

public class PaginatedEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> values;
    public readonly int? StartOfPage;
    public readonly int? CountPerPage;
    public readonly int OriginCount;
    public readonly int CurrentPageIndex;
    public readonly int TotalPageCounts;

    public PaginatedEnumerable(
        IEnumerable<T> values,
        int? startOfPage,
        int? countPerPage,
        int originCount)
    {
        this.values = values;
        this.StartOfPage = startOfPage;
        this.CountPerPage = countPerPage;
        this.OriginCount = originCount;

        this.CurrentPageIndex = this.StartOfPage.HasValue && this.CountPerPage.HasValue ?
            this.StartOfPage.Value / this.CountPerPage.Value : 0;
        this.TotalPageCounts = this.CountPerPage.HasValue ?
            (int)Math.Ceiling((double)this.OriginCount / this.CountPerPage.Value) : 1;
    }

    public IEnumerator<T> GetEnumerator() => this.values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
