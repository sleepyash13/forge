using Forge.Application.DTOs.Auth;

namespace Forge.Application.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<LoginResult> LoginAsync(LoginRequest request);
    }
}
