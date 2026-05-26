using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Enums;
namespace HelpDeskManager.Core.DTOs.Customer;

public sealed record CustomerDto
{
    public required Guid Id { get; init; }
    public required DocumentType DocumentType { get; init; }
    public required string DocumentNumber { get; init; }
    public required string CustomerFullName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public List<SupportRequestDetailsDto> SupportRequests { get; init; } = [];
}
