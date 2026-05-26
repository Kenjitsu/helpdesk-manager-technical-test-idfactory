using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Entities;

namespace HelpDeskManager.Core.Interfaces.Repositories;

public interface ISupportRequestRepository
{
    Task<SupportRequest?> GetByIdAsync(Guid id);
    Task<SupportRequestDetailsDto?> GetByIdWithDetailsAsync(Guid id);
    Task<PaginatedResult<SupportRequestDetailsDto>> GetByCriteriaAsync(RequestSearchCriteriaDto criteria);
    void Add(SupportRequest request);
    void Update(SupportRequest request);
    void AddComment(Comment comment);
    void AddHistoryRecord(RequestHistory history);
}
