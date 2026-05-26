using HelpDeskManager.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskManager.Core.DTOs.Account;

public record RegisterRequestDto
{
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DocumentType DocumentType { get; init; }
    public string DocumentNumber { get; init; } = string.Empty;
    public IList<string> Roles { get; init; } = [];
}
