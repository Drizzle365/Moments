using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Moments.Model;

namespace Moments.Service;

/// <summary>
/// 权限认证服务
/// </summary>
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _storage;
    private readonly ConfigService _configService;

    public CustomAuthenticationStateProvider(ProtectedLocalStorage storage, ConfigService configService)
    {
        _storage = storage;
        _configService = configService;
    }

    /// <summary>
    /// 获取用户权限状态
    /// </summary>
    /// <returns></returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userLocalStorage = await _storage.GetAsync<UserSession>("identity");
            var token = _configService.Get("Token");
            if (userLocalStorage.Value is null || token != userLocalStorage.Value.Token)
            {
                return new AuthenticationState(new ClaimsPrincipal(CreateIdentityFromUser(null)));
            }

            var principal = CreateIdentityFromUser(userLocalStorage.Success ? userLocalStorage.Value : null);
            return new AuthenticationState(new ClaimsPrincipal(principal));
        }
        catch (Exception)
        {
            return new AuthenticationState(new ClaimsPrincipal(CreateIdentityFromUser(null)));
        }
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="userSession"></param>
    public async void Login(UserSession userSession)
    {
        await _storage.SetAsync("identity", userSession);
        var identity = CreateIdentityFromUser(userSession);
        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    /// <summary>
    /// 登出
    /// </summary>
    public async void LogOut()
    {
        await _storage.SetAsync("identity", new UserSession { Role = "" });
        var identity = CreateIdentityFromUser(null);
        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    /// <summary>
    /// 创建用户令牌
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
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