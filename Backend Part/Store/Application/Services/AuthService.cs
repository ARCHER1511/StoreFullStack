using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IRefreshTokenService _refresh;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IUnitOfWork _uow;

        public AuthService(
            IUserRepository users,
            IRefreshTokenService refresh,
            IJwtTokenGenerator jwt,
            IUnitOfWork uow
        )
        {
            _users = users;
            _refresh = refresh;
            _jwt = jwt;
            _uow = uow;
        }

        public async Task<(User user, string accessToken, string refreshToken)>
        RegisterAsync(RegisterRequest request)
        {
            
            if (await _users.ExistsAsync(u => u.UserName == request.UserName))
                throw new InvalidOperationException("Username already taken.");

            if (await _users.ExistsAsync(u => u.Email == request.Email))
                throw new InvalidOperationException("Email already registered.");

            
            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = hash,
                LastLoginTime = DateTime.UtcNow
            };
            await _users.AddAsync(user);

            var accessToken = _jwt.GenerateAccessToken(user);
            var refresh = await _refresh.CreateAsync(user.Id);

            return (user, accessToken, refresh.Token);
        }

        public async Task<(string accessToken, RefreshToken refreshToken)> LoginAsync(string username, string password)
        {
            var user = await _users.GetByUserNameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            user.LastLoginTime = DateTime.Now;
            _users.Update(user);
            await _uow.SaveChangesAsync();

            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = await _refresh.CreateAsync(user.Id);
            return (accessToken, refreshToken);
        }

        public async Task<(string accessToken, RefreshToken refreshToken)> RefreshAsync(string userId, string token)
        {
            var valid = await _refresh.ValidateAsync(userId, token);
            if (valid == null) throw new UnauthorizedAccessException("Invalid refresh token");

            var user = await _users.GetByIdAsync(userId) ?? throw new KeyNotFoundException();
            var newAccess = _jwt.GenerateAccessToken(user);
            var newRefresh = await _refresh.CreateAsync(userId);

            await _refresh.RevokeAsync(token);
            return (newAccess, newRefresh);
        }
    }
}
