using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.DTOs.Results;

namespace HelpDeskManager.Core.Interfaces.Services;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
    Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
}
