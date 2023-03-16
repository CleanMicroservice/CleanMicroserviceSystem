using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanMicroserviceSystem.Astra.Infrastructure.Services;

public class AstraStringArrayComparer : ValueComparer<string[]>
{
    public static readonly AstraStringArrayComparer Instance = new();

    public AstraStringArrayComparer()
        : base((c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToArray())
    {
    }
}
