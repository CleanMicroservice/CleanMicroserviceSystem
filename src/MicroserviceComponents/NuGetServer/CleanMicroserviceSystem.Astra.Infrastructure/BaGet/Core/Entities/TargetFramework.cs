namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Entities
{
    public class TargetFramework
    {
        public int Key { get; set; }

        public string Moniker { get; set; }

        public virtual Package Package { get; set; }
    }
}
