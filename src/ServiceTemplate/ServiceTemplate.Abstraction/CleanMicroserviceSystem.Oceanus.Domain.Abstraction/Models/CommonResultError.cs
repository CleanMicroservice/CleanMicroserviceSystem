namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Models;

public class CommonResultError
{
    public CommonResultError()
        : this(null, null)
    {
    }

    public CommonResultError(string? message)
        : this(null, message)
    {
    }

    public CommonResultError(string? code, string? message)
    {
        this.Code = code;
        this.Message = message;
    }

    public string? Code { get; set; }

    public string? Message { get; set; }

    public override string ToString()
    {
        return $"{(string.IsNullOrEmpty(this.Code) ? string.Empty : $"{this.Code} - ")}{this.Message}";
    }
}
