using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Moments.Model;

namespace Moments.Service;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _storage;

    public CustomAuthenticationStateProvider(ProtectedLocalStorage storage)
    {
        _storage = storage;
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userLocalStorage = await _storage.GetAsync<UserSession>("identity");
            var principal = CreateIdentityFromUser(userLocalStorage.Success ? userLocalStorage.Value : null);
            return new AuthenticationState(new ClaimsPrincipal(principal));
        }
        catch (Exception)
        {
            return new AuthenticationState(new ClaimsPrincipal(CreateIdentityFromUser(null)));
        }
    }

    public async void Login(UserSession userSession)
    {
        await _storage.SetAsync("identity", userSession);
        var identity = CreateIdentityFromUser(userSession);
        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async void LogOut()
    {
        await _storage.SetAsync("identity", new UserSession { Role = "" });
        var identity = CreateIdentityFromUser(null);
        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }


    private static ClaimsPrincipal CreateIdentityFromUser(UserSession? user)
    {
        if (user is null) return new ClaimsPrincipal();
        var claims = new List<Claim>
        {
            new("Token", user.Token!)
        };
        claims.AddRange(user.Role!.Split(",").Select(p => new Claim(ClaimTypes.Role, p)));
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        return claimsPrincipal;
    }
}