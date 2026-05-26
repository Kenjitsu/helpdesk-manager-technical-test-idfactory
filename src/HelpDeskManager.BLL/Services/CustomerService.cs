using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Customer;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Services;
using HelpDeskManager.Core.Mappers;
using System.Net;

namespace HelpDeskManager.BLL.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CustomerResponseDto>> GetCustomerByDocumentNumberAsync(string documentNumber)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByDocumentAsync(documentNumber);
        if (customer == null)
        {
            return Result<CustomerResponseDto>.Failure(new Error("CUSTOMER_NOT_FOUND", "No customer found with the provided document number."), HttpStatusCode.NotFound);
        }

        return Result<CustomerResponseDto>.Success(customer);
    }

    public async Task<Result<CustomerResponseDto>> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return Result<CustomerResponseDto>.Failure(new Error("CUSTOMER_NOT_FOUND", "No customer found with the provided ID."), HttpStatusCode.NotFound);
        }

        return Result<CustomerResponseDto>.Success(customer.ToCustomerResponseDto());
    }

    public async Task<Result<PaginatedResult<CustomerResponseDto>>> GetCustomersAsync(int pageNumber, int pageSize)
    {
        var pagedCustomers = await _unitOfWork.CustomerRepository.GetPagedAsync(pageNumber, pageSize);

        if (pagedCustomers == null)
        {
            return Result<PaginatedResult<CustomerResponseDto>>.Failure(new Error("CUSTOMERS_NOT_FOUND", "No customers found."), HttpStatusCode.NotFound);
        }

        return Result<PaginatedResult<CustomerResponseDto>>.Success(pagedCustomers, HttpStatusCode.Created);
    }


    public async Task<Result<CustomerResponseDto>> CreateCustomerAsync(CreateCustomerDto customerDto)
    {
        var newCustomer = customerDto.ToCustomerEnitty();

        _unitOfWork.CustomerRepository.Add(newCustomer);

        var result = await _unitOfWork.SaveChangesAsync();

        if(!result)
        {
            return Result<CustomerResponseDto>.Failure(new Error("ERROR_CREATING_CUSTOMER", "Failed to create the customer."));
        }

        var createdCustomerDto = newCustomer.ToCustomerResponseDto();

        return Result<CustomerResponseDto>.Success(createdCustomerDto);
    }

    public async Task<Result<bool>> DeleteCustomerAsync(Guid id)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            return Result<bool>.Failure(new Error("USER_NOT_FOUND", "No customer found with the provided ID."), HttpStatusCode.NotFound);
        }

        _unitOfWork.CustomerRepository.Delete(customer);
        var result = await _unitOfWork.SaveChangesAsync();

        if(!result)
        {
            return Result<bool>.Failure(new Error("ERROR_DELETING_CUSTOMER", "Failed to delete the customer."));
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<CustomerResponseDto>> UpdateCustomerAsync(Guid id, UpdateCustomerDto customerDto)
    {
        var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            return Result<CustomerResponseDto>.Failure(new Error("CUSTOMER_NOT_FOUND", "No customer found with the provided ID."), HttpStatusCode.NotFound);
        }

        customer.UpdateCustomerFromDto(customerDto);

        var result = await _unitOfWork.SaveChangesAsync();

        if(!result)
        {
            return Result<CustomerResponseDto>.Failure(new Error("ERROR_UPDATING_CUSTOMER", "Failed to update the customer."));
        }

        return Result<CustomerResponseDto>.Success(customer.ToCustomerResponseDto());
    }
}
