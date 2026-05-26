using System.ComponentModel.DataAnnotations;

namespace HelpDeskManager.Core.DTOs.Account;

public record LoginRequestDto
{
    [EmailAddress]
    [Required]
    public required string Email { get; init; }

    [Required]
    public required string Password { get; init; }
}

public record LoginResponseDto()
{
    public required string AccessToken { get; init; }
}
