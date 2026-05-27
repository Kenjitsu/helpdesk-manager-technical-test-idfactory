using HelpDeskManager.Core.DTOs.Customer;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskManager.API.Controllers;

public class CustomersController : BaseApiController
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [Authorize(Policy = "ReadDataRole")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);

        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "ReadDataRole")]
    [HttpGet("document/{documentNumber}")]
    [ProducesResponseType(typeof(Result<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CustomerDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCustomerByDocumentNumber(string documentNumber)
    {
        var result = await _customerService.GetCustomerByDocumentNumberAsync(documentNumber);

        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "ReadDataRole")]
    [HttpGet]
    [ProducesResponseType(typeof(Result<CustomerResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CustomerResponseDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCustomers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _customerService.GetCustomersAsync(pageNumber, pageSize);

        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "WriteDataRole")]
    [HttpPost]
    [ProducesResponseType(typeof(Result<CustomerResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CustomerResponseDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto customerDto)
    {
        var result = await _customerService.CreateCustomerAsync(customerDto);

        return result.Match<IActionResult>(
            onSuccess: success => CreatedAtAction(nameof(GetCustomerById), new { id = success.Data!.Id }, success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "WriteDataRole")]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<CustomerResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CustomerResponseDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<CustomerResponseDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerDto customerDto)
    {
        var result = await _customerService.UpdateCustomerAsync(id, customerDto);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        
        return result.Match<IActionResult>(
            onSuccess: success => NoContent(),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }


}
