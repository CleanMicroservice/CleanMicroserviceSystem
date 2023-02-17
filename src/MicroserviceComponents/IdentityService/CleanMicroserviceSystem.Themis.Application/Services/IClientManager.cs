using CleanMicroserviceSystem.Themis.Application.DataTransferObjects.ApiScopes;
using CleanMicroserviceSystem.Themis.Application.Models;

namespace CleanMicroserviceSystem.Themis.Application.Services
{
    public interface IClientManager
    {
        Task<IEnumerable<ApiScopeInformationResponse>?> GetClientApiScopes(int clientId);

        Task<ClientSigninResult> SignInAsync(string clientName, string clientSecret);
    }
}
