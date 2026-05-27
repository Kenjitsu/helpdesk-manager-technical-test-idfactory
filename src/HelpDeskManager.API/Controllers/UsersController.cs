using HelpDeskManager.Core.Common.Pagination;
using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.DTOs.Results;
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
    [ProducesResponseType(typeof(Result<PaginatedResult<AppUserDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _userService.GetUsersAsync(pageNumber, pageSize);

        return result.Match<IActionResult>(
                onSuccess: success => Ok(success),
                onFailure: error => StatusCode(error.StatusCode, error)
            );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<AppUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetRoles(string id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [HttpGet("document/{documentNumber}")]
    [ProducesResponseType(typeof(Result<AppUserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetUserByDocumentNumber(string documentNumber)
    {
        var result = await _userService.GetUserByDocumentNumberAsync(documentNumber);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [HttpGet("roles")]
    [ProducesResponseType(typeof(Result<IEnumerable<string>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetRoles()
    {
        var result = await _userService.GetRolesAsync();
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [HttpPost("roles")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AssignRole([FromBody] AssingRoleRequestDto assignRoleRequest)
    {
        var result = await _userService.AssignRoleAsync(assignRoleRequest);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }
}
