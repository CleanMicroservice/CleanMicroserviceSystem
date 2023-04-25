using CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Contracts;

namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Entities;

public class WebAPILog : AuditableEntity<int>
{
    public string RequestURI { get; set; } = default!;

    public string QueryString { get; set; } = default!;

    public string Method { get; set; } = default!;

    public string SourceHost { get; set; } = default!;

    public string? UserAgent { get; set; }

    public string TraceIdentifier { get; set; } = default!;

    public bool IsAuthenticated { get; set; }

    public string? IdentityName { get; set; }

    public string? RequestBody { get; set; }

    public string? ResponseBody { get; set; }

    public int StatusCode { get; set; }

    public long ElapsedTime { get; set; }

    public string? Exception { get; set; }
}
