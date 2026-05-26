using HelpDeskManager.Core.DTOs.Account;

namespace HelpDeskManager.Core.Interfaces.Services;

public interface ITokenService
{
    Task<string> CreateToken(AppUserDto userDto);
}
