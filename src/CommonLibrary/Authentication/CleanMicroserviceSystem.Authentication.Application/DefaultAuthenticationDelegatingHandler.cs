using System.Net.Http.Headers;
using CleanMicroserviceSystem.Authentication.Application;
using CleanMicroserviceSystem.Authentication.Domain;

namespace CleanMicroserviceSystem.Aphrodite.Infrastructure.Services.Authentication;

public class DefaultAuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IAuthenticationTokenStore tokenStore;

    public DefaultAuthenticationDelegatingHandler(
        IAuthenticationTokenStore tokenStore)
    {
        this.tokenStore = tokenStore;
    }

    protected override HttpResponseMessage Send(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        this.AttachAuthorizationHeader(request).ConfigureAwait(false);
        return base.Send(request, cancellationToken);
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await this.AttachAuthorizationHeader(request);
        return await base.SendAsync(request, cancellationToken);
    }

    protected async Task AttachAuthorizationHeader(HttpRequestMessage request)
    {
        if (request.Headers.Authorization is null)
        {
            var token = await this.tokenStore.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return;
            }
            request.Headers.Authorization = new AuthenticationHeaderValue(IdentityContract.BearerScheme, token);
        }
    }
}
