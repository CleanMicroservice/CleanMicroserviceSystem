using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities.Converters
{
    public class UriToStringConverter : ValueConverter<Uri, string>
    {
        public static readonly UriToStringConverter Instance = new UriToStringConverter();

        public UriToStringConverter()
            : base(
                v => v.AbsoluteUri,
                v => new Uri(v))
        {
        }
    }
}
