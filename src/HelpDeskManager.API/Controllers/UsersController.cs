using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskManager.API.Controllers;

[Authorize(Policy = "RequireAdminRole")]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _userService.GetUsersAsync(pageNumber, pageSize);

        return result.Match<IActionResult>(
                onSuccess: success => Ok(success),
                onFailure: error => NotFound(error)
            );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => NotFound(error)
        );
    }

    [HttpGet("document/{documentNumber}")]
    public async Task<IActionResult> GetUserByDocumentNumber(string documentNumber)
    {
        var result = await _userService.GetUserByDocumentNumberAsync(documentNumber);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => NotFound(error)
        );
    }

    [HttpPost("roles")]
    public async Task<IActionResult> AssignRole([FromBody] AssingRoleRequestDto assignRoleRequest)
    {
        var result = await _userService.AssignRoleAsync(assignRoleRequest);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode((int)error.StatusCode, error)
        );
    }

    

    




}
