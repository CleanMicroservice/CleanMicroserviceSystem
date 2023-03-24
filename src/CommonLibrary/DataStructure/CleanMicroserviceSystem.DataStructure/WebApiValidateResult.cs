namespace CleanMicroserviceSystem.DataStructure;

public class WebApiValidateResult
{
    public Dictionary<string, IEnumerable<string>> Errors { get; set; } = new();
}
