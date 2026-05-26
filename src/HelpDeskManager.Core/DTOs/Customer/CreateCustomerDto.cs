using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Customer;

public sealed record class CreateCustomerDto
{
    public required DocumentType DocumentType { get; init; }
    public required string DocumentNumber { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}
