using IlkProjem.Core.Dtos.AuthDtos;

namespace IlkProjem.BLL.Interfaces;
public interface IAuthService
{
    string Login(LoginDto loginDto);
}
