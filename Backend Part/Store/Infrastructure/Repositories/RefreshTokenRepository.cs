using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(StoreDbContext context)
            : base(context) { }

        public async Task<RefreshToken?> GetValidTokenAsync(string userId, string token)
        {
            return await _dbSet.FirstOrDefaultAsync(rt =>
                rt.UserId == userId
                && rt.Token == token
                && rt.Expires > DateTime.Now
                && !rt.IsRevoked
            );
        }
    }
}
