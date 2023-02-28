namespace CleanMicroserviceSystem.Oceanus.Domain.Abstraction.Models;

public class CommonResult<T>
{
    public T? Entity { get; set; }

    public bool Succeeded => string.IsNullOrEmpty(this.Error);

    public string? Error { get; set; }
}
