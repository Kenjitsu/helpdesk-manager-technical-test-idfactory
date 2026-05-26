using HelpDeskManager.Core.DTOs.Customer;
using HelpDeskManager.Core.Entities;

namespace HelpDeskManager.Core.Mappers;

public static class CustomerMapperExtensions
{
    public static CustomerDto ToCustomerDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            DocumentType = customer.DocumentType,
            DocumentNumber = customer.DocumentNumber,
            CustomerFullName = $"{customer.FirstName} {customer.LastName}",
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            SupportRequests = customer.SupportRequests.Select(sr => sr.ToSupportRequestDetailsDto()).ToList()
        };
    }

    public static Customer ToCustomer(this CreateCustomerDto dto)
    {
        return new Customer
        {
            DocumentType = dto.DocumentType,
            DocumentNumber = dto.DocumentNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
        };
    }

    public static Customer ToCustomer(this CustomerDto dto)
    {
        var names = dto.CustomerFullName.Split(' ', 2);
        return new Customer
        {
            Id = dto.Id,
            DocumentType = dto.DocumentType,
            DocumentNumber = dto.DocumentNumber,
            FirstName = names[0],
            LastName = names.Length > 1 ? names[1] : string.Empty,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };
    }

    public static CustomerResponseDto ToCustomerResponseDto(this Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            DocumentType = customer.DocumentType,
            DocumentNumber = customer.DocumentNumber,
            CustomerFullName = $"{customer.FirstName} {customer.LastName}",
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };
    }

    public static void UpdateCustomerFromDto(this Customer customer, UpdateCustomerDto dto)
    {
        customer.DocumentType = dto.DocumentType ?? customer.DocumentType;
        customer.DocumentNumber = dto.DocumentNumber ?? customer.DocumentNumber;
        customer.FirstName = dto.FirstName ?? customer.FirstName;
        customer.LastName = dto.LastName ?? customer.LastName;
        customer.Email = dto.Email ?? customer.Email;
        customer.PhoneNumber = dto.PhoneNumber ?? customer.PhoneNumber;
    }
}
