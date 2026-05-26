using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Customer;

public sealed class UpdateCustomerDto
{
    public DocumentType? DocumentType { get; init; }
    public string? DocumentNumber { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
}
