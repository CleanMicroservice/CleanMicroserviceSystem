using System.Threading;
using System.Threading.Tasks;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Core.Authentication
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateAsync(string apiKey, CancellationToken cancellationToken);
    }
}
