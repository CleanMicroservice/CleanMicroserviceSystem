using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities.Converters;

public class StringArrayComparer : ValueComparer<string[]>
{
    public static readonly StringArrayComparer Instance = new();

    public StringArrayComparer()
        : base(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToArray())
    {
    }
}