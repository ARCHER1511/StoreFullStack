using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRefreshTokenService 
    {
        Task<RefreshToken> CreateAsync(string userId);
        Task<RefreshToken?> ValidateAsync(string userId, string token);
        Task RevokeAsync(string token);
    }
}
