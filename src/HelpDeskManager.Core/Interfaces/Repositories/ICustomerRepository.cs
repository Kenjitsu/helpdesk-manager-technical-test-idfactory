using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Customer;
using HelpDeskManager.Core.Entities;

namespace HelpDeskManager.Core.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<CustomerDto?> GetByDocumentAsync(string documentNumber);
    Task<PaginatedResult<CustomerDto>> GetPagedAsync(int pageNumber, int pageSize);
    void AddAsync(Customer customer);
    void UpdateAsync(Customer customer);
    void DeleteAsync(Customer customer);
}
