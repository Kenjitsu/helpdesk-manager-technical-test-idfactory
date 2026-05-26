namespace HelpDeskManager.Core.DTOs.Support;

public record CommentDto
{
    public Guid Id { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
