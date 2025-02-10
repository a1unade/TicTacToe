using Microsoft.AspNetCore.Http;
using TicTacToe.Application.Interfaces;

namespace TicTacToe.Application.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthorizationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public (bool isValid, string message) Authorization()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity.IsAuthenticated)
        {
            return (false, "User is not authenticated.");
        }

        return (true, "Authorization successful.");
    }
}