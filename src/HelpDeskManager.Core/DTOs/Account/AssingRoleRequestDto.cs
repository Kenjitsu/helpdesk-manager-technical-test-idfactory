using System.ComponentModel.DataAnnotations;

namespace HelpDeskManager.Core.DTOs.Account;

public record AssingRoleRequestDto
{
    [EmailAddress]
    [Required]
    public required string Email { get; init; }

    [Required]
    public required string Role { get; init; }
}
