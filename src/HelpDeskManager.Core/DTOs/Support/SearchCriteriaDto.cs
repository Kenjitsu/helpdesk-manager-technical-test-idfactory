using HelpDeskManager.Core.Enums;

namespace HelpDeskManager.Core.DTOs.Support;

public record RequestSearchCriteriaDto
{
    public Guid? CustomerId { get; init; }
    public RequestStatus? Status { get; init; }
    public RequestType? Type { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
