using HelpDeskManager.Core.Enums;
using HelpDeskManager.Core.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace HelpDeskManager.DAL.Data.Identity;

public class AppUser : IdentityUser, IAuditableEntity, ISoftDeletable
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DocumentType DocumentType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
