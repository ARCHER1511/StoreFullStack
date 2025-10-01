using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<(User user, string accessToken, string refreshToken)> RegisterAsync(RegisterRequest request);
        Task<(string accessToken, RefreshToken refreshToken)> LoginAsync(string username, string password);
        Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(string userId, string refreshToken);
    }
}
