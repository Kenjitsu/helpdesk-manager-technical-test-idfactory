using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.DTOs.Support;

namespace HelpDeskManager.Core.Interfaces.Services;

public interface ISupportRequestService
{
    Task<Result<Guid>> CreateRequestAsync(CreateSupportRequestDto createSupportRequestDto);
    Task<Result<string>> ChangeStatusAsync(Guid requestId, string modifiedByUserId, ChangeSupportRequestStatusDto changeSupportRequestStatusDto);
    Task<Result<PaginatedResult<SupportRequestDetailsDto>>> GetRequestsByCriteriaAsync(RequestSearchCriteriaDto searchCriteria);
    Task<Result<SupportRequestDetailsDto>> GetRequestByIdAsync(Guid requestId);
    Task<Result<string>> AddCommentAsync(string authorId, CreateCommentDto createCommentDto);
    Task<Result<SupportRequestDetailsDto>> UpdateAsync(Guid id, UpdateSupportRequestDto updateSupportRequestDto);
}
