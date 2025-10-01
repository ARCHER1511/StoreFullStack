using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly IUserService _users;

        public AuthController(IAuthService auth, IUserService users)
        {
            _auth = auth;
            _users = users;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _auth.RegisterAsync(request);

            return Ok(new
            {
                userId = result.user.Id,
                accessToken = result.accessToken,
                refreshToken = result.refreshToken
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var (access, refresh) = await _auth.LoginAsync(dto.UserName, dto.Password);
                var user = await _users.GetByUserNameAsync(dto.UserName);

                return Ok(new {
                    userId = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    accessToken = access, 
                    refreshToken = refresh.Token
                });
            }
            catch
            {
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequestDto dto)
        {
            try
            {
                var (access, refresh) = await _auth.RefreshAsync(dto.UserId, dto.RefreshToken);
                return Ok(new { accessToken = access, refreshToken = refresh.Token });
            }
            catch
            {
                return Unauthorized("Invalid refresh token");
            }
        }
    }
}
