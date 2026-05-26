using HelpDeskManager.Core.Interfaces.Entities;

namespace HelpDeskManager.Core.Entities;

public class Comment : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid SupportRequestId { get; set; }
    public string UserId { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public SupportRequest SupportRequest { get; set; } = null!;
}
