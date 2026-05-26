using HelpDeskManager.Core.Enums;
using System;
namespace HelpDeskManager.Core.DTOs.Support;

public record SupportRequestDetailsDto
{
    public required Guid Id { get; init; }
    public required RequestType Type { get; init; }
    public required string Subject { get; init; } = string.Empty;
    public required RequestStatus Status { get; init; }
    public required string CustomerFullName { get; init; } = string.Empty;
    public required DateTime CreatedAt { get; init  ; }
    public List<CommentDto> Comments { get; init; } = [];
    public List<RequestHistoryDto> History { get; init; } = [];
}
