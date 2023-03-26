namespace CleanMicroserviceSystem.DataStructure;

public class WebApiValidateResult
{
    public Dictionary<string, IEnumerable<string>> Errors { get; set; } = new();

    public override string ToString()
    {
        return string.Join("; ", this.Errors.Select(fieldErrors => $"{fieldErrors.Key} : {string.Join("; ", fieldErrors.Value)}"));
    }
}
