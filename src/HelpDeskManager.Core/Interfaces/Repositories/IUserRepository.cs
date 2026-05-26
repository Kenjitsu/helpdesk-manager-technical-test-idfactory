using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Account;

namespace HelpDeskManager.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<AppUserDto?> GetByIdAsync(string id);
    Task<AppUserDto?> GetByEmailAsync(string email);
    Task<AppUserDto?> GetByDocumentNumberAsync(string documentNumber);
    Task<(bool IsValid, AppUserDto? User)> ValidateCredentialsAsync(string email, string password);
    Task<(bool Success, IEnumerable<string> Errors)> CreateUserAsync(string email, string password, AppUserDto userDto);
    Task<(bool Success, IEnumerable<string> Errors)> AddToRoleAsync(string email, string roleName);
    Task<PaginatedResult<AppUserDto>> GetPagedAsync(int pageNumber, int pageSize);
    Task<IList<string>> GetAssignedUserRolesAsync(AppUserDto userDto);
    Task<IList<string?>> GetRolesAsync();
}
