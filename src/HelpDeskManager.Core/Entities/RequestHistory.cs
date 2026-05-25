using HelpDeskManager.Core.Enums;
using HelpDeskManager.Core.Interfaces.Entities;

namespace HelpDeskManager.Core.Entities;

public class RequestHistory : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid SupportRequestId { get; set; }
    public RequestStatus? PreviousStatus { get; set; }
    public RequestStatus NewStatus { get; set; }
    public string ChangeNotes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string ModifiedByUserId { get; set; } = string.Empty;
    public SupportRequest SupportRequest { get; set; } = null!;
}
