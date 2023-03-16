using System.Net;

namespace CleanMicroserviceSystem.Astra.Infrastructure.BaGet.Protocol.Models;

public class ProtocolException : Exception
{
    public ProtocolException(
        string message,
        HttpMethod method,
        string requestUri,
        HttpStatusCode statusCode,
        string reasonPhrase) : base(message)
    {
        this.Method = method ?? throw new ArgumentNullException(nameof(method));
        this.RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        this.StatusCode = statusCode;
        this.ReasonPhrase = reasonPhrase ?? throw new ArgumentNullException(nameof(reasonPhrase));
    }

    public HttpMethod Method { get; }

    public string RequestUri { get; }

    public HttpStatusCode StatusCode { get; }

    public string ReasonPhrase { get; }
}