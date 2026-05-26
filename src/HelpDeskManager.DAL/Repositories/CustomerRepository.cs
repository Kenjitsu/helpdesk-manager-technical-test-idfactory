using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Customer;
using HelpDeskManager.Core.Entities;
using HelpDeskManager.Core.Interfaces.Repositories;
using HelpDeskManager.DAL.Data;
using HelpDeskManager.DAL.Helpers;
using HelpDeskManager.DAL.Projections;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskManager.DAL.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;
    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
    }

    public async Task<CustomerResponseDto?> GetByDocumentAsync(string documentNumber)
    {
        return await _context.Customers
            .Where(c => c.DocumentNumber == documentNumber)
            .ProjectToCustomerResponseDto()
            .FirstOrDefaultAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<PaginatedResult<CustomerResponseDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _context.Customers
            .Include(c => c.SupportRequests)
            .AsNoTracking()
            .ProjectToCustomerResponseDto()
            .ToPaginatedResultAsync(pageNumber, pageSize);
    }

    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public void Delete(Customer customer)
    {
        _context.Customers.Remove(customer);
    }
}
