namespace CleanMicroserviceSystem.DataStructure;

public class PaginatedEnumerable<TItem>
{
    public static PaginatedEnumerable<TItem> Empty => new(Enumerable.Empty<TItem>(), 0, 0, 0);

    public IEnumerable<TItem> Values { get; set; }
    public int? StartItemIndex { get; protected set; }
    public int? PageSize { get; protected set; }
    public int OriginItemCount { get; protected set; }
    public int CurrentPageIndex { get; protected set; }
    public int TotalPageCount { get; protected set; }

    public PaginatedEnumerable(
        IEnumerable<TItem> values,
        int? startItemIndex,
        int? pageSize,
        int originItemCount)
    {
        this.Values = values;
        this.StartItemIndex = startItemIndex;
        this.PageSize = pageSize;
        this.OriginItemCount = originItemCount;

        this.CurrentPageIndex = this.StartItemIndex.HasValue && this.PageSize.HasValue && this.PageSize > 0 ?
            this.StartItemIndex.Value / this.PageSize.Value : 0;
        this.TotalPageCount = this.PageSize.HasValue && this.PageSize > 0 ?
            (int)Math.Ceiling((double)this.OriginItemCount / this.PageSize.Value) : 1;
    }
}
