using HelpDeskManager.Core.Enums;
using HelpDeskManager.Core.Interfaces.Entities;

namespace HelpDeskManager.Core.Entities;

public class SupportRequest : IAuditableEntity, ISoftDeletable
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public RequestType Type { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Customer Customer { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<RequestHistory> History { get; set; } = [];

}
