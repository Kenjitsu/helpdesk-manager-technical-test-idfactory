using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Support;

public sealed record CreateSupportRequestDto
{
    public required Guid CustomerId { get; set; }
    public required RequestType Type { get; init; }
    public required string Subject { get; init; } = string.Empty;
    public required string Description { get; init; } = string.Empty;
    public required RequestStatus Status { get; init; }
}
