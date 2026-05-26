using HelpDeskManager.Core.DTOs.Customer;
using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Entities;

namespace HelpDeskManager.DAL.Projections;

public static class CustomerProjections
{
    public static IQueryable<CustomerDto> ProjectToCustomerDto(this IQueryable<Customer> query)
    {
        return query.Select(c => new CustomerDto
        {
            Id = c.Id,
            DocumentType = c.DocumentType,
            DocumentNumber = c.DocumentNumber,
            CustomerFullName = $"{c.FirstName} {c.LastName}",
            Email = c.Email,
            PhoneNumber = c.PhoneNumber,
            SupportRequests = c.SupportRequests.Select(sr => new SupportRequestDetailsDto
            {
                Id = sr.Id,
                Type = sr.Type,
                Subject = sr.Subject,
                CustomerFullName = $"{c.FirstName} {c.LastName}",
                Status = sr.Status,
                CreatedAt = sr.CreatedAt
            }).ToList()
        });
    }
}
