namespace HelpDeskManager.Core.DTOs.Support;

public sealed record CreateCommentDto
{
    public required Guid RequestId { get; init; }
    public required string Message { get; init; }
}
