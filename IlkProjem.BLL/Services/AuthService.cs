using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IlkProjem.DAL.Data;
using IlkProjem.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using IlkProjem.Core.Dtos.AuthDtos;
using IlkProjem.Core.Models;

namespace IlkProjem.BLL.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public string Login(LoginDto loginDto)
    {
        // 1. Kullanıcıyı email ile bul
        var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);
        
        // 2. Kullanıcı yoksa veya şifre yanlışsa (BCrypt kontrolü)
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new Exception("Email veya şifre hatalı!");
        }

        // 3. Her şey doğruysa Token üret
        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // Admin/Staff yetkisi için
            }),
            Expires = DateTime.UtcNow.AddDays(1), // Token 1 gün geçerli
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}