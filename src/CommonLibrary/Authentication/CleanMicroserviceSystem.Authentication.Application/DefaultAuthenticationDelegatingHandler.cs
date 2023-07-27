using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using CleanMicroserviceSystem.Authentication.Domain;
using Microsoft.Extensions.Http.Logging;

namespace CleanMicroserviceSystem.Authentication.Application;

public class DefaultAuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IAuthenticationTokenStore tokenStore;
    private static readonly Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool> serverCertificateCustomValidationCallback = (_, _, _, _) => true;
    private bool handledUpdated = false;

    public DefaultAuthenticationDelegatingHandler(
        HttpMessageHandler handler,
        IAuthenticationTokenStore tokenStore)
        : base(handler)
    {
        this.tokenStore = tokenStore;
    }

    public DefaultAuthenticationDelegatingHandler(
        IAuthenticationTokenStore tokenStore)
        : base()
    {
        this.tokenStore = tokenStore;
    }

    protected override HttpResponseMessage Send(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        this.SetupServerCertificateCustomValidationCallback();
        this.AttachAuthorizationHeader(request).ConfigureAwait(false);
        return base.Send(request, cancellationToken);
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        this.SetupServerCertificateCustomValidationCallback();
        await this.AttachAuthorizationHeader(request);
        return await base.SendAsync(request, cancellationToken);
    }

    protected void SetupServerCertificateCustomValidationCallback()
    {
        if (this.handledUpdated) return;
        lock (this)
        {
            if (this.handledUpdated) return;
            this.handledUpdated = true;
            try
            {
                var innerHandler = this.InnerHandler;
                if (innerHandler is LoggingHttpMessageHandler loggingHandler)
                {
                    innerHandler = loggingHandler.InnerHandler;
                }
                if (innerHandler is HttpClientHandler handler)
                {
                    handler.ServerCertificateCustomValidationCallback = serverCertificateCustomValidationCallback;
                }
            }
            catch
            {
            }
        }
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
