using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IlkProjem.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using IlkProjem.Core.Dtos.AuthDtos;
using IlkProjem.Core.Models;
using IlkProjem.Core.Utilities.Results;
using Microsoft.Extensions.Localization;
using IlkProjem.Core.Resources;
using IlkProjem.DAL.Repositories;

namespace IlkProjem.BLL.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IStringLocalizer<Messages> _localizer; // Localization eklendi

    public AuthService(IConfiguration configuration, IUserRepository userRepository, IStringLocalizer<Messages> localizer)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _localizer = localizer;
    }

    public async Task<IDataResult<string>> Login(LoginDto loginDto, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email, ct);
        
        // Localizer kullanarak hata mesajı dönüyoruz
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return new ErrorDataResult<string>(_localizer["AuthError"]); 
        }

        var token = GenerateJwtToken(user);
        return new SuccessDataResult<string>(token, _localizer["LoginSuccess"]);
    }

    public async Task<IResult> Register(RegisterDto registerDto, CancellationToken ct = default)
    {
        if (await _userRepository.ExistsByEmailAsync(registerDto.Email, ct))
        {
            return new ErrorResult(_localizer["EmailAlreadyExists"]);
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var newUser = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = hashedPassword,
            Role = "Staff", // User modelindeki Role alanı
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(newUser, ct);
        await _userRepository.SaveChangesAsync(ct);

        return new SuccessResult(_localizer["RegisterSuccess"]);
    }

    private string GenerateJwtToken(User user)
    {
        // Önceki adımda yaptığımız JwtKey null kontrolü burada devam etmeli
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing");
        var key = Encoding.ASCII.GetBytes(jwtKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) 
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}