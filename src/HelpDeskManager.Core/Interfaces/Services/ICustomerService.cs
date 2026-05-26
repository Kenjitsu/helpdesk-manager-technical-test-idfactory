using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Customer;
using HelpDeskManager.Core.DTOs.Results;

namespace HelpDeskManager.Core.Interfaces.Services;

public interface ICustomerService
{
    Task<Result<CustomerResponseDto>> GetCustomerByIdAsync(Guid id);
    Task<Result<CustomerResponseDto>> GetCustomerByDocumentNumberAsync(string documentNumber);
    Task<Result<PaginatedResult<CustomerResponseDto>>> GetCustomersAsync(int pageNumber, int pageSize);
    Task<Result<CustomerResponseDto>> CreateCustomerAsync(CreateCustomerDto customerDto);
    Task<Result<CustomerResponseDto>> UpdateCustomerAsync(Guid id, UpdateCustomerDto customerDto);
    Task<Result<bool>> DeleteCustomerAsync(Guid id);
}
