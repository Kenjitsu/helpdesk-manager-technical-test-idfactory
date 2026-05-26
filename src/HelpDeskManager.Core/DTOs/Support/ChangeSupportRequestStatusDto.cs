using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Support;

public sealed record ChangeSupportRequestStatusDto
{
    public required RequestStatus NewStatus { get; init; }
    public required string Notes { get; init; }
}
