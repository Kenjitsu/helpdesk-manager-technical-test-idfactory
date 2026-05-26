using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Customer;
using HelpDeskManager.Core.Entities;

namespace HelpDeskManager.Core.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id);
    Task<CustomerResponseDto?> GetByDocumentAsync(string documentNumber);
    Task<PaginatedResult<CustomerResponseDto>> GetPagedAsync(int pageNumber, int pageSize);
    void Add(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
}
