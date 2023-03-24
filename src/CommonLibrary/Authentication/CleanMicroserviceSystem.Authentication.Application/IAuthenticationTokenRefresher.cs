namespace CleanMicroserviceSystem.Authentication.Application;

public interface IAuthenticationTokenRefresher
{
    bool IsRunning { get; }

    void StartRefresher();

    void StopRefresher();

    Task RefreshTokenAsync();
}
