using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Entities;

namespace HelpDeskManager.Core.Interfaces.Repositories;

public interface ISupportRequestRepository
{
    Task<SupportRequest?> GetByIdAsync(Guid id);
    Task<SupportRequestDetailsDto?> GetByIdWithDetailsAsync(Guid id);
    Task<PaginatedResult<SupportRequestDetailsDto>> GetByCriteriaAsync(RequestSearchCriteriaDto criteria);
    void AddAsync(SupportRequest request);
    void UpdateAsync(SupportRequest request);
    void AddCommentAsync(Comment comment);
    void AddHistoryRecordAsync(RequestHistory history);
}
