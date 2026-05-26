using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.Interfaces.Repositories;
using HelpDeskManager.DAL.Data;
using HelpDeskManager.DAL.Data.Identity;
using HelpDeskManager.DAL.Helpers;
using HelpDeskManager.DAL.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskManager.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;

    public UserRepository(UserManager<AppUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<(bool Success, IEnumerable<string> Errors)> AddToRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return (false, new[] { "User not found" });
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        if(!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description));
        }

        return (true, Array.Empty<string>());
    }

    public async Task<(bool IsValid, AppUserDto? User)> ValidateCredentialsAsync(string email, string password)
    {
        var userEntity = await _userManager.FindByEmailAsync(email);

        if (userEntity == null)
        {
            return (false, null);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(userEntity, password);

        if (!isPasswordValid)
        {
            return (false, null);
        }

        var roles = await _userManager.GetRolesAsync(userEntity);
        var userDto = userEntity.ToAppUserDto(roles);

        return (true, userDto);
    }

    public async Task<(bool Success, IEnumerable<string> Errors)> CreateUserAsync(string email, string password, AppUserDto userDto)
    {
        var userExist = await _userManager.FindByEmailAsync(email);

        if (userExist != null)
        {
            return (false, new[] { "User already exists" });
        }

        var user = userDto.ToAppUserEntity();

        var result = await _userManager.CreateAsync(user, password);

        if (userDto.Roles != null && userDto.Roles.Any())
        {
            var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(userExist!);

                return (false, roleResult.Errors.Select(e => e.Description));
            }
        }

        return (true, Array.Empty<string>());
    }

    public async Task<AppUserDto?> GetByDocumentNumberAsync(string documentNumber)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.DocumentNumber == documentNumber);

        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return user.ToAppUserDto(roles);
    }

    public async Task<AppUserDto?> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return user.ToAppUserDto(roles);
    }

    public async Task<AppUserDto?> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);

        return user.ToAppUserDto(roles);
    }

    public async Task<PaginatedResult<AppUserDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = GetAppUserWithRoles();

        return await query.AsNoTracking()
            .OrderByDescending(u => u.CreatedAt)
            .ToPaginatedResultAsync(pageNumber, pageSize);
    }

    public async Task<IList<string>> GetAssignedUserRolesAsync(AppUserDto userDto)
    {
        var user = userDto.ToAppUserEntity();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IList<string?>> GetRolesAsync()
    {
        return await _context.Roles
            .AsNoTracking()
            .Select(r => r.Name)
            .ToListAsync();
    }

    private IQueryable<AppUserDto> GetAppUserWithRoles()
    {
        var query = from user in _context.Users

                    let userRoles = (from ur in _context.UserRoles
                                     join r in _context.Roles on ur.RoleId equals r.Id
                                     where ur.UserId == user.Id
                                     select r.Name).ToList()
                    select new AppUserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email!,
                        DocumentType = user.DocumentType,
                        DocumentNumber = user.DocumentNumber,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                        DeletedAt = user.DeletedAt,
                        Roles = userRoles
                    };

        return query;
    }
}
