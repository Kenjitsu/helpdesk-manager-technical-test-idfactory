using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Support;

public sealed record CreateOrUpdateRequestHistoryDto
{
    public RequestStatus? PreviousStatus { get; init; }
    public RequestStatus NewStatus { get; init; }
    public string ChangeNotes { get; init; } = string.Empty;
    public string ModifiedByUserId { get; init; } = string.Empty;
}
