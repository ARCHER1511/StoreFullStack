using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        // For specific methods related to RefreshToken entity
        Task<RefreshToken?> GetValidTokenAsync(string userId, string token);
    }
}
