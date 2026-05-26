using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Services;
using System.Net;

namespace HelpDeskManager.BLL.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IList<string?>>> GetRolesAsync()
    {
        var roles = await _unitOfWork.UserRepository.GetRolesAsync();

        if(roles == null)
        {
            return Result<IList<string?>>.Failure(new Error("NO_ROLES_FOUND", "No roles found in the system."), HttpStatusCode.NotFound);
        }

        return Result<IList<string?>>.Success(roles);
    }

    public async Task<Result<AppUserDto>> GetUserByDocumentNumberAsync(string documentNumber)
    {
        var user = await _unitOfWork.UserRepository.GetByDocumentNumberAsync(documentNumber);

        if (user == null)
        {
            return Result<AppUserDto>.Failure(new Error("USER_NOT_FOUND", "User not found."), HttpStatusCode.NotFound);
        }

        return Result<AppUserDto>.Success(user);
    }

    public async Task<Result<AppUserDto>> GetUserByIdAsync(string id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

        if (user == null)
        {
            return Result<AppUserDto>.Failure(new Error("USER_NOT_FOUND", "User not found in the system."), HttpStatusCode.NotFound);
        }

        return Result<AppUserDto>.Success(user);
    }

    public async Task<Result<PaginatedResult<AppUserDto>>> GetUsersAsync(int pageNumber, int pageSize)
    {
        var pagedUsers = await _unitOfWork.UserRepository.GetPagedAsync(pageNumber, pageSize);

        if (pagedUsers == null)
        {
            return Result<PaginatedResult<AppUserDto>>.Failure(new Error("USERS_NOT_FOUND", "No users found."), HttpStatusCode.NotFound);
        }

        return Result<PaginatedResult<AppUserDto>>.Success(pagedUsers);
    }

    public async Task<Result<string>> AssignRoleAsync(AssingRoleRequestDto assignRoleRequestDto)
    {
        var (Success, Errors) = await _unitOfWork.UserRepository.AddToRoleAsync(assignRoleRequestDto.Email, assignRoleRequestDto.Role);

        if (!Success)
        {
            var errorMessages = string.Join(", ", Errors);
            return Result<string>.Failure(new Error("ASSIGN_ROLE_FAILED", $"Failed to assign role: {errorMessages}"), HttpStatusCode.BadRequest);
        }

        return Result<string>.Success("Role assigned successfully.");
    }
}
