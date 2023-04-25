using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Ocelot.Configuration;
using Ocelot.Request.Mapper;
using Ocelot.Responses;

namespace CleanMicroserviceSystem.Uranus.Infrastructure.Middlewares;
public class UranusRequestMapper : IRequestMapper
{
    private readonly string[] _unsupportedHeaders = { "host" };
    private readonly ILogger<UranusRequestMapper> logger;

    public UranusRequestMapper(
        ILogger<UranusRequestMapper> logger)
    {
        this.logger = logger;
    }

    public async Task<Response<HttpRequestMessage>> Map(HttpRequest request, DownstreamRoute downstreamRoute)
    {
        try
        {
            var requestMessage = new HttpRequestMessage()
            {
                Content = await this.MapContent(request),
                Method = this.MapMethod(request, downstreamRoute),
                RequestUri = this.MapUri(request),
                Version = downstreamRoute.DownstreamHttpVersion,
            };

            this.MapHeaders(request, requestMessage);

            return new OkResponse<HttpRequestMessage>(requestMessage);
        }
        catch (Exception ex)
        {
            return new ErrorResponse<HttpRequestMessage>(new UnmappableRequestError(ex));
        }
    }

    private async Task<HttpContent?> MapContent(HttpRequest request)
    {
        if (request.Body == null)
        {
            return null;
        }

        var content = new ByteArrayContent(await this.ToByteArray(request.Body));

        if (!string.IsNullOrEmpty(request.ContentType))
        {
            _ = content.Headers
                .TryAddWithoutValidation("Content-Type", new[] { request.ContentType });
        }

        this.AddHeaderIfExistsOnRequest("Content-Language", content, request);
        this.AddHeaderIfExistsOnRequest("Content-Location", content, request);
        this.AddHeaderIfExistsOnRequest("Content-Range", content, request);
        this.AddHeaderIfExistsOnRequest("Content-MD5", content, request);
        this.AddHeaderIfExistsOnRequest("Content-Disposition", content, request);
        this.AddHeaderIfExistsOnRequest("Content-Encoding", content, request);

        return content;
    }

    private void AddHeaderIfExistsOnRequest(string key, HttpContent content, HttpRequest request)
    {
        if (request.Headers.ContainsKey(key))
        {
            _ = content.Headers
                .TryAddWithoutValidation(key, request.Headers[key].ToList());
        }
    }

    private HttpMethod MapMethod(HttpRequest request, DownstreamRoute downstreamRoute)
    {
        return !string.IsNullOrEmpty(downstreamRoute?.DownstreamHttpMethod)
            ? new HttpMethod(downstreamRoute.DownstreamHttpMethod)
            : new HttpMethod(request.Method);
    }

    private Uri MapUri(HttpRequest request)
    {
        return new Uri(request.GetEncodedUrl());
    }

    private void MapHeaders(HttpRequest request, HttpRequestMessage requestMessage)
    {
        foreach (var header in request.Headers)
        {
            if (this.IsSupportedHeader(header))
            {
                _ = requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
    }

    private bool IsSupportedHeader(KeyValuePair<string, StringValues> header)
    {
        return !this._unsupportedHeaders.Contains(header.Key.ToLower());
    }

    private async Task<byte[]> ToByteArray(Stream stream)
    {
        using var memStream = new MemoryStream();
        await stream.CopyToAsync(memStream);
        return memStream.ToArray();
    }
}
