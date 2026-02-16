using Microsoft.AspNetCore.Mvc;
using IlkProjem.BLL.Services;
using IlkProjem.Core.Dtos;
using IlkProjem.BLL.Interfaces;
using IlkProjem.Core.Dtos.AuthDtos;

[ApiController]
[Route("api/[controller]")] // Burası api/Auth yapar
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")] // Burası api/Auth/login yapar
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        try 
        {
            var token = _authService.Login(loginDto);
            return Ok(new { Token = token }); // Angular buradaki 'Token'ı bekliyor
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}