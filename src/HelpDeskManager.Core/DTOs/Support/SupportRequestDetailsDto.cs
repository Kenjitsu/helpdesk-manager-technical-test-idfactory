using HelpDeskManager.Core.Enums;
using System;
namespace HelpDeskManager.Core.DTOs.Support;

public record SupportRequestDetailsDto
{
    public required Guid Id { get; init; }
    public required Guid CustomerId { get; init; }
    public required string CustomerFullName { get; init; } = string.Empty;
    public required RequestType Type { get; init; }
    public required string Subject { get; init; } = string.Empty;
    public required string Description { get; init; } = string.Empty;
    public required RequestStatus Status { get; init; }
    public required DateTime CreatedAt { get; init  ; }
    public IEnumerable<CommentDto> Comments { get; set; } = [];
    public IEnumerable<RequestHistoryDto> History { get; set; } = [];
}
