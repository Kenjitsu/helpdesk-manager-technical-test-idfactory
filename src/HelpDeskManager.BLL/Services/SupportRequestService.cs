using Azure.Core;
using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Entities;
using HelpDeskManager.Core.Enums;
using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Services;
using HelpDeskManager.Core.Mappers;
using System.Net;

namespace HelpDeskManager.BLL.Services;

public class SupportRequestService : ISupportRequestService
{

    private readonly IUnitOfWork _unitOfWork;

    public SupportRequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> AddCommentAsync(string authorId, CreateCommentDto createCommentDto)
    {
        var request = await _unitOfWork.SupportRequestRepository.GetByIdAsync(createCommentDto.RequestId);

        if(request == null || request.Status == RequestStatus.Closed)
        {
            return Result<string>.Failure(new Error("INVALID_REQUEST", "Cannot add comment to a closed or non-existent request."), HttpStatusCode.BadRequest);
        }

        var comment = createCommentDto.ToCommentEntity(authorId);

        _unitOfWork.SupportRequestRepository.AddComment(comment);
        var success = await _unitOfWork.SaveChangesAsync();

        if (!success)
            return Result<string>.Failure(new Error("COMMENT_CREATION_FAILED", "Failed to add comment."), HttpStatusCode.InternalServerError);

        return Result<string>.Success("Comment added successfully.");
    }

    public async Task<Result<string>> ChangeStatusAsync(Guid requestId, string modifiedByUserId, ChangeSupportRequestStatusDto changeSupportRequestStatusDto)
    {
        var request = await _unitOfWork.SupportRequestRepository.GetByIdAsync(requestId);

        if (request == null)
            return Result<string>.Failure(new Error("SUPPORT_REQUEST_NOT_FOUND", "Support request not found."), HttpStatusCode.NotFound);

        if (request.Status == RequestStatus.Closed)
            return Result<string>.Failure(new Error("INVALID_REQUEST", "Cannot change status of a closed request."), HttpStatusCode.BadRequest);


        if (request.Status == changeSupportRequestStatusDto.NewStatus)
            return Result<string>.Failure(new Error("INVALID_STATUS", "The request is already in the specified status."), HttpStatusCode.BadRequest);

        if (changeSupportRequestStatusDto.NewStatus == RequestStatus.Closed && request.Status != RequestStatus.Resolved)
            return Result<string>.Failure(
                new Error("INVALID_STATUS_TRANSITION", "A request must be resolved before it can be closed."), HttpStatusCode.BadRequest);

        var previousStatus = request.Status;

        request.Status = changeSupportRequestStatusDto.NewStatus;

        var historyRecord = BuildHistoryRecord(request, changeSupportRequestStatusDto, modifiedByUserId, previousStatus);

        _unitOfWork.SupportRequestRepository.Update(request);
        _unitOfWork.SupportRequestRepository.AddHistoryRecord(historyRecord);

        var success = await _unitOfWork.SaveChangesAsync();

        if (!success)
            return Result<string>.Failure(new Error("HISTORY_REQUEST_ERROR", "Could not save the status change."), HttpStatusCode.InternalServerError);

        return Result<string>.Success("Status changed successfully.");
    }

    public async Task<Result<Guid>> CreateRequestAsync(CreateSupportRequestDto createSupportRequestDto, string creatorId)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(createSupportRequestDto.CustomerId);

        if (customer == null)
        {
            return Result<Guid>.Failure(new Error("CUSTOMER_NOT_FOUND", "Cannot create request, customer not found."), HttpStatusCode.NotFound);
        }

        var supportRequest = createSupportRequestDto.ToSupportRequestEntity(customer, creatorId);

        _unitOfWork.SupportRequestRepository.Add(supportRequest);

        var result = await _unitOfWork.SaveChangesAsync();

        if (!result)
        {
            return Result<Guid>.Failure(new Error("SUPPORT_REQUEST_CREATION_FAILED", "Failed to create support request."), HttpStatusCode.InternalServerError);
        }

        return Result<Guid>.Success(supportRequest.Id);
    }

    public async Task<Result<SupportRequestDetailsDto>> GetRequestByIdAsync(Guid requestId)
    {
        var request = await _unitOfWork.SupportRequestRepository.GetByIdWithDetailsAsync(requestId);

        if (request == null)
        {
            return Result<SupportRequestDetailsDto>.Failure(new Error("SUPPORT_REQUEST_NOT_FOUND", "Support request not found."), HttpStatusCode.NotFound);
        }

        return Result<SupportRequestDetailsDto>.Success(request);
    }

    public async Task<Result<PaginatedResult<SupportRequestDetailsDto>>> GetRequestsByCriteriaAsync(RequestSearchCriteriaDto searchCriteria)
    {
        var paginatedRequests = await _unitOfWork.SupportRequestRepository.GetByCriteriaAsync(searchCriteria);

        if(paginatedRequests == null)
        {
            return Result<PaginatedResult<SupportRequestDetailsDto>>.Failure(new Error("SUPPORT_REQUESTS_NOT_FOUND", "No support requests found."), HttpStatusCode.NotFound);
        }

        return Result<PaginatedResult<SupportRequestDetailsDto>>.Success(paginatedRequests);
    }

    public async Task<Result<SupportRequestDetailsDto>> UpdateAsync(Guid id, UpdateSupportRequestDto updateSupportRequestDto)
    {
        var request = await _unitOfWork.SupportRequestRepository.GetByIdAsync(id);

        if (request == null)
        {
            return Result<SupportRequestDetailsDto>.Failure(new Error("SUPPORT_REQUEST_NOT_FOUND", "Support request not found."), HttpStatusCode.NotFound);
        }

        if (request.Status == RequestStatus.Closed)
        {
            return Result<SupportRequestDetailsDto>.Failure(new Error("REQUEST_CLOSED", "Cannot modify a closed support request."), HttpStatusCode.BadRequest);
        }

        request.UpdateSupportRequestEntity(updateSupportRequestDto);

        _unitOfWork.SupportRequestRepository.Update(request);

        var result = await _unitOfWork.SaveChangesAsync();

        if (!result)
        {
            return Result<SupportRequestDetailsDto>.Failure(new Error("SUPPORT_REQUEST_UPDATE_FAILED", "Failed to update support request."), HttpStatusCode.InternalServerError);
        }

        var updatedRequestDetailsDto = await _unitOfWork.SupportRequestRepository.GetByIdWithDetailsAsync(id);

        return Result<SupportRequestDetailsDto>.Success(updatedRequestDetailsDto!);
    }

    private static RequestHistory BuildHistoryRecord(SupportRequest request, ChangeSupportRequestStatusDto dto, string userId, RequestStatus previousStatus)
    {
        return new RequestHistory
        {
            SupportRequestId = request.Id,
            PreviousStatus = previousStatus,
            NewStatus = dto.NewStatus,
            ChangeNotes = dto.Notes,
            ModifiedByUserId = userId,
        };
    }
}
