using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.DAL.Data.Seeding.DTOs;

public class SeedDataDto
{
    public List<SeedUserDto> Users { get; set; } = [];
    public List<SeedCustomerDto> Customers { get; set; } = [];
    public List<SeedRequestDto> SupportRequests { get; set; } = [];
    public List<SeedRequestHistoryDto> RequestHistories { get; set; } = [];
}

public class SeedUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DocumentType DocumentType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class SeedCustomerDto
{
    public Guid Id { get; set; }
    public DocumentType DocumentType { get; set; }
    public string DocumentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class SeedRequestDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public RequestType Type { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RequestStatus Status { get; set; }
    public List<SeedCommentDto> Comments { get; set; } = [];
}

public class SeedCommentDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class SeedRequestHistoryDto
{
    public Guid Id { get; set; }
    public Guid SupportRequestId { get; set; }
    public RequestStatus? PreviousStatus { get; set; }
    public RequestStatus NewStatus { get; set; }
    public string ChangeNotes { get; set; } = string.Empty;
    public string ModifiedByUserId { get; set; } = string.Empty;
}
