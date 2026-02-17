using Microsoft.AspNetCore.Mvc;

using IlkProjem.BLL.Interfaces;
using IlkProjem.Core.Dtos.AuthDtos;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")] // Burası api/Auth yapar
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")] // api/Auth/register
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            // BLL tarafında kayıt işlemini gerçekleştiriyoruz
            await _authService.Register(registerDto);
            return Ok(new { message = "Kayıt işlemi başarıyla tamamlandı!" });
        }
        catch (Exception ex)
        {
            // E-posta zaten varsa veya başka bir hata oluşursa BLL'den fırlatılan mesajı döner
            return BadRequest(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken ct)
    {
        // 'await' kullanmazsan Task objesinin kendisi döner (F12'deki hata budur)
        var result = await _authService.Login(loginDto, ct); 

        if (result.Success)
        {
            return Ok(result); // Artık sadece { success: true, message: "...", data: "token" } dönecek
        }
        return BadRequest(result);
    }
}