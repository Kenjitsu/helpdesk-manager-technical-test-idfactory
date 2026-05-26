using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Entities;

namespace HelpDeskManager.Core.Mappers;

public static class SupportRequestMapperExtensions
{
    public static SupportRequestDetailsDto ToSupportRequestDetailsDto(this SupportRequest supportRequest)
    {
        return new SupportRequestDetailsDto
        {
            Id = supportRequest.Id,
            CustomerId = supportRequest.CustomerId,
            CustomerFullName = $"{supportRequest.Customer.FirstName} {supportRequest.Customer.LastName}",
            Subject = supportRequest.Subject,
            Description = supportRequest.Description,
            Type = supportRequest.Type,
            Status = supportRequest.Status,
            CreatedAt = supportRequest.CreatedAt,
            Comments = supportRequest.Comments.Select(c => new CommentDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Message = c.Message,
                CreatedAt = c.CreatedAt,
            }),
            History = supportRequest.History.Select(h => new RequestHistoryDto
            {
                Id = h.Id,
                PreviousStatus = h.PreviousStatus,
                NewStatus = h.NewStatus,
                ChangeNotes = h.ChangeNotes,
                ModifiedByUserId = h.ModifiedByUserId,
                UpdatedAt = h.UpdatedAt,
            }),
        };
    }

    public static SupportRequest ToSupportRequestEntity(this SupportRequestDetailsDto supportRequestDetailsDto, Customer customer)
    {
        return new SupportRequest
        {
            Id = supportRequestDetailsDto.Id,
            CustomerId = customer.Id,
            Status = supportRequestDetailsDto.Status,
            Type = supportRequestDetailsDto.Type,
            Subject = supportRequestDetailsDto.Subject,
            Description = supportRequestDetailsDto.Description,
            Customer = customer,
            Comments = supportRequestDetailsDto.Comments.Select(c => new Comment
            {
                Id = c.Id,
                UserId = c.UserId,
                Message = c.Message,
                CreatedAt = c.CreatedAt,
            }).ToList(),
            History = supportRequestDetailsDto.History.Select(h => new RequestHistory
            {
                Id = h.Id,
                PreviousStatus = h.PreviousStatus,
                NewStatus = h.NewStatus,
                ChangeNotes = h.ChangeNotes,
                ModifiedByUserId = h.ModifiedByUserId,
                UpdatedAt = h.UpdatedAt,
            }).ToList(),
        };
    }

    public static SupportRequest ToSupportRequestEntity(this CreateSupportRequestDto dto, Customer customer)
    {
        return new SupportRequest
        {
            Subject = dto.Subject,
            Description = dto.Description,
            Type = dto.Type,
            Status = dto.Status,
            CustomerId = customer.Id,
            Customer = customer
        };
    }

    public static void UpdateSupportRequestEntity(this SupportRequest supportRequest, UpdateSupportRequestDto updateSupportRequestDto)
    {
        supportRequest.Subject = updateSupportRequestDto.Subject ?? supportRequest.Subject;
        supportRequest.Description = updateSupportRequestDto.Description ?? supportRequest.Description;
        supportRequest.Type = updateSupportRequestDto.Type ?? supportRequest.Type;
    }

    public static Comment ToCommentEntity(this CreateCommentDto createCommentDto, string authorId)
    {
        return new Comment
        {
            SupportRequestId = createCommentDto.RequestId,
            UserId = authorId,
            Message = createCommentDto.Message,
        };
    }
}
