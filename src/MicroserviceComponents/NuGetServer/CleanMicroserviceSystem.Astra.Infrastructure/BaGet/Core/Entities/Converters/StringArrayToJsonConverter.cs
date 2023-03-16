using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities.Converters
{
    public class StringArrayToJsonConverter : ValueConverter<string[], string>
    {
        public static readonly StringArrayToJsonConverter Instance = new StringArrayToJsonConverter();

        public StringArrayToJsonConverter()
            : base(
                v => JsonConvert.SerializeObject(v),
                v => !string.IsNullOrEmpty(v) ? JsonConvert.DeserializeObject<string[]>(v) : new string[0])
        {
        }
    }
}
