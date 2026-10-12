using Microsoft.EntityFrameworkCore;
using PetHaven.Api.Authentication;
using PetHaven.Api.Data;
using PetHaven.Api.DTOs.Auth;
using PetHaven.Api.DTOs.Common;
using PetHaven.Api.Helpers;
using PetHaven.Api.Models;
using PetHaven.Api.Services.Interfaces;

namespace PetHaven.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IJwtTokenGenerator jwtTokenGenerator, ILogger<AuthService> logger)
    {
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        var emailLower = dto.Email.Trim().ToLowerInvariant();
        var existingUser = await _context.Users.AnyAsync(u => u.Email == emailLower);
        if (existingUser)
        {
            return ApiResponse<AuthResponseDto>.Fail("A user with this email address already exists.", 409);
        }

        if (!string.IsNullOrWhiteSpace(dto.Phone))
        {
            var existingPhone = await _context.Users.AnyAsync(u => u.Phone == dto.Phone.Trim());
            if (existingPhone)
            {
                return ApiResponse<AuthResponseDto>.Fail("A user with this phone number already exists.", 409);
            }
        }

        var user = new User
        {
            Name = dto.Name.Trim(),
            Email = emailLower,
            Phone = dto.Phone?.Trim(),
            PasswordHash = PasswordHasher.HashPassword(dto.Password),
            Role = "customer",
            Status = "active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Create empty Cart and Wishlist for user
        _context.Carts.Add(new Cart { UserId = user.Id });
        _context.Wishlists.Add(new Wishlist { UserId = user.Id });
        await _context.SaveChangesAsync();

        var (token, expiresAt) = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        var response = new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = MapUserToDto(user)
        };

        return ApiResponse<AuthResponseDto>.Created(response, "Registration successful");
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var emailLower = dto.Email.Trim().ToLowerInvariant();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailLower);

        if (user == null || !PasswordHasher.VerifyPassword(dto.Password, user.PasswordHash))
        {
            return ApiResponse<AuthResponseDto>.Fail("Invalid email or password credentials.", 401);
        }

        if (user.Status != "active")
        {
            return ApiResponse<AuthResponseDto>.Fail($"Account is {user.Status}. Please contact support.", 403);
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var (token, expiresAt) = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        var response = new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = MapUserToDto(user)
        };

        return ApiResponse<AuthResponseDto>.Ok(response, "Login successful");
    }

    public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
    {
        // For stateless JWT demonstration with refresh tokens
        return await Task.FromResult(ApiResponse<AuthResponseDto>.Fail("Refresh token flow invoked. Please authenticate with credentials.", 400));
    }

    public async Task<ApiResponse<bool>> LogoutAsync(ulong userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        return ApiResponse<bool>.Ok(true, "Logged out successfully");
    }

    private static UserInfoDto MapUserToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Phone = user.Phone,
        Role = user.Role,
        Status = user.Status
    };
}
