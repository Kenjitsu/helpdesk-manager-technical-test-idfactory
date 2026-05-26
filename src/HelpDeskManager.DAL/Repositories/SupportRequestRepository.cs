using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Entities;
using HelpDeskManager.Core.Interfaces.Repositories;
using HelpDeskManager.DAL.Data;
using HelpDeskManager.DAL.Helpers;
using HelpDeskManager.DAL.Projections;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskManager.DAL.Repositories;

public class SupportRequestRepository : ISupportRequestRepository
{
    private readonly AppDbContext _context;

    public SupportRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public void AddAsync(SupportRequest request)
    {
        _context.SupportRequests.Add(request);
    }

    public void AddCommentAsync(Comment comment)
    {
        _context.Comments.Add(comment);
    }

    public void AddHistoryRecordAsync(RequestHistory history)
    {
        _context.RequestHistories.Add(history);
    }

    public async Task<PaginatedResult<SupportRequestDetailsDto>> GetByCriteriaAsync(RequestSearchCriteriaDto criteria)
    {
        var query = _context.SupportRequests.AsNoTracking();

        if (criteria.CustomerId.HasValue)
            query = query.Where(sr => sr.CustomerId == criteria.CustomerId.Value);

        if (criteria.Status.HasValue)
            query = query.Where(sr => sr.Status == criteria.Status.Value);

        if (criteria.Type.HasValue)
            query = query.Where(sr => sr.Type == criteria.Type.Value);

        query = query.OrderByDescending(sr => sr.CreatedAt);

        return await query
            .ProjectToDetails()
            .ToPaginatedResultAsync(criteria.PageNumber, criteria.PageSize);
    }

    public async Task<SupportRequest?> GetByIdAsync(Guid id)
    {
        return await _context.SupportRequests.FindAsync(id);
    }

    public Task<SupportRequestDetailsDto?> GetByIdWithDetailsAsync(Guid id)
    {
        return _context.SupportRequests
            .AsNoTracking()
            .Where(sr =>sr.Id == id)
            .ProjectToDetails()
            .FirstOrDefaultAsync();
    }

    public void UpdateAsync(SupportRequest request)
    {
        _context.SupportRequests.Update(request);
    }
}
