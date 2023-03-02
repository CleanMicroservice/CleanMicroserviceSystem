namespace CleanMicroserviceSystem.Authentication.Application;

public interface IAuthenticationTokenStore
{
    event EventHandler<string> TokenUpdated;

    ValueTask<string> GetTokenAsync();

    Task UpdateTokenAsync(string token);
}
