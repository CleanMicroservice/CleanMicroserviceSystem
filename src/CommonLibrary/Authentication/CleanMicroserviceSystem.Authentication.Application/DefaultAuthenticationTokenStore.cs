namespace CleanMicroserviceSystem.Authentication.Application;

public class DefaultAuthenticationTokenStore : IAuthenticationTokenStore
{
    public event EventHandler<string>? TokenUpdated;

    private string token = default!;

    public ValueTask<string> GetTokenAsync()
    {
        return ValueTask.FromResult(token);
    }

    public Task UpdateTokenAsync(string token)
    {
        this.token = token;
        return Task.CompletedTask;
    }
}
