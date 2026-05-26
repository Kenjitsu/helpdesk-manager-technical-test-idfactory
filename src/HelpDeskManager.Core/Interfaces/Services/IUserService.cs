using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.DTOs.Results;

namespace HelpDeskManager.Core.Interfaces.Services;

public interface IUserService
{
    Task<Result<AppUserDto>> GetUserByIdAsync(string id);
    Task<Result<AppUserDto>> GetUserByDocumentNumberAsync(string documentNumber);
    Task<Result<PaginatedResult<AppUserDto>>> GetUsersAsync(int pageNumber, int pageSize);
    Task<Result<IList<string?>>> GetRolesAsync();
    Task<Result<string>> AssignRoleAsync(AssingRoleRequestDto assignRoleRequestDto);
}
