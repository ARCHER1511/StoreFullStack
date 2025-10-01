using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Security.Cryptography;

namespace Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _repo;
        private readonly IUnitOfWork _uow;

        public RefreshTokenService(IRefreshTokenRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<RefreshToken> CreateAsync(string userId)
        {
            var token = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                IsRevoked = false,
                UserId = userId,
            };

            await _repo.AddAsync(token);
            await _uow.SaveChangesAsync();
            return token;
        }

        public async Task<RefreshToken?> ValidateAsync(string userId, string token)
        {
            var refresh = await _repo.GetValidTokenAsync(userId, token);
            return (refresh != null && refresh.Expires > DateTime.Now && !refresh.IsRevoked)
                ? refresh
                : null;
        }

        public async Task RevokeAsync(string token)
        {
            var refresh = await _repo.FindAsync(r => r.Token == token);
            var entity = refresh.FirstOrDefault();
            if (entity == null)
                return;
            entity.IsRevoked = true;
            _repo.Update(entity);
            await _uow.SaveChangesAsync();
        }
    }
}
