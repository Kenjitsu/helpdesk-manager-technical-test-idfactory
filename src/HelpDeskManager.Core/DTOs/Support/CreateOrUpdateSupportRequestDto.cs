using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Support;

public sealed record CreateOrUpdateSupportRequestDto
{
    public required RequestType Type { get; init; }
    public required string Subject { get; init; } = string.Empty;
    public required string Description { get; init; } = string.Empty;
    public required RequestStatus Status { get; init; }
    public required string FirstName { get; init; } = string.Empty;
    public required string LastName { get; init; } = string.Empty;
    public List<CommentDto> Comments { get; init; } = [];
    public List<RequestHistoryDto> History { get; init; } = [];
}
