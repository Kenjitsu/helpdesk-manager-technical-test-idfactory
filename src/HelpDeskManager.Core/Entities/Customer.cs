using HelpDeskManager.Core.Enums;
using HelpDeskManager.Core.Interfaces.Entities;

namespace HelpDeskManager.Core.Entities;

public class Customer : IAuditableEntity, ISoftDeletable
{
    public Guid Id { get; set; }
    public DocumentType DocumentType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public ICollection<SupportRequest> SupportRequests { get; set; } = [];

}
