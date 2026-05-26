namespace HelpDeskManager.Core.DTOs.Support;

public sealed record CreateOrUpdateCommentDto
{
    public string UserId { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
