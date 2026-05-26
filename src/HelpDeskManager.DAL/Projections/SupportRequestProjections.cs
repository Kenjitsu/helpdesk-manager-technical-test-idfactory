using HelpDeskManager.Core.DTOs;
using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Entities;

namespace HelpDeskManager.DAL.Projections;

public static class SupportRequestProjections
{
    public static IQueryable<SupportRequestDetailsDto> ProjectToDetails(this IQueryable<SupportRequest> query)
    {
        return query.Select(sr => new SupportRequestDetailsDto
        {
            Id = sr.Id,
            Type = sr.Type,
            Subject = sr.Subject,
            Description = sr.Description,
            Status = sr.Status,
            CustomerFullName = $"{sr.Customer.FirstName} {sr.Customer.LastName}",
            CreatedAt = sr.CreatedAt,

            Comments = sr.Comments.Select(c => new CommentDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Message = c.Message,
                CreatedAt = c.CreatedAt
            }).ToList(),

            History = sr.History.Select(h => new RequestHistoryDto
            {
                Id = h.Id,
                PreviousStatus = h.PreviousStatus,
                NewStatus = h.NewStatus,
                ChangeNotes = h.ChangeNotes,
                ModifiedByUserId = h.ModifiedByUserId,
                UpdatedAt = h.UpdatedAt
            }).ToList()
        });
    }
}