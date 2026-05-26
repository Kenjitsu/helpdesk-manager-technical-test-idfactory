using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.DAL.Data.Identity;

namespace HelpDeskManager.DAL.Mappers;

public static class AppUserMapperExtensions
{
    public static AppUserDto ToAppUserDto(this AppUser user, IList<string> roles)
    {
        return new AppUserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            DocumentNumber = user.DocumentNumber,
            DocumentType = user.DocumentType,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            DeletedAt = user.DeletedAt,
            Roles = roles
        };
    }

    public static AppUser ToAppUserEntity(this AppUserDto userDto)
    {
        return new AppUser
        {
            Id = userDto.Id,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            DocumentNumber = userDto.DocumentNumber,
            DocumentType = userDto.DocumentType,
            CreatedAt = userDto.CreatedAt,
            UpdatedAt = userDto.UpdatedAt,
            DeletedAt = userDto.DeletedAt,
            UserName = userDto.Email,
        };
    }

    public static AppUserDto ToAppUserDto(this RegisterRequestDto registerRequestDto)
    {
        return new AppUserDto
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = registerRequestDto.FirstName,
            LastName = registerRequestDto.LastName,
            Email = registerRequestDto.Email,
            DocumentType = registerRequestDto.DocumentType,
            DocumentNumber = registerRequestDto.DocumentNumber,
            CreatedAt = DateTime.UtcNow,
            Roles = registerRequestDto.Roles
        };
    }
}
