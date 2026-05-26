using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Support;

public record RequestHistoryDto
{
    public Guid Id { get; init; }
    public RequestStatus? PreviousStatus { get; init; }
    public RequestStatus NewStatus { get; init; }
    public string ChangeNotes { get; init; } = string.Empty;
    public string ModifiedByUserId { get; init; } = string.Empty;
    public DateTime? UpdatedAt { get; init; }
}
