using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Support;

public record UpdateSupportRequestDto
{
    public RequestType? Type { get; init; }
    public string? Subject { get; init; }
    public string? Description { get; init; }
}
