using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Account;

public record AppUserDto
{
    public required string Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public required string Email { get; init; }
    public DocumentType DocumentType { get; init; }
    public string DocumentNumber { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? DeletedAt { get; init; }
    public IList<string> Roles { get; init; } = [];
}
