using IlkProjem.Core.Dtos.AuthDtos;
using IlkProjem.Core.Utilities.Results;

namespace IlkProjem.BLL.Interfaces;

public interface IAuthService
{
    Task<IDataResult<string>> Login(LoginDto loginDto, CancellationToken ct = default);
    Task<IResult> Register(RegisterDto registerDto, CancellationToken ct = default);
}