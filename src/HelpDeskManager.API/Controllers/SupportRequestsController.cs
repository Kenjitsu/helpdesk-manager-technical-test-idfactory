using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace HelpDeskManager.API.Controllers;

public class SupportRequestsController : BaseApiController
{
    private readonly ISupportRequestService _supportRequestService;

    public SupportRequestsController(ISupportRequestService supportRequestService)
    {
        _supportRequestService = supportRequestService;
    }

    [Authorize(Policy = "ReadDataRole")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<SupportRequestDetailsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<SupportRequestDetailsDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetSupportRequestById(Guid id)
    {
        var result = await _supportRequestService.GetRequestByIdAsync(id);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "ReadDataRole")]
    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<SupportRequestDetailsDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<IEnumerable<SupportRequestDetailsDto>>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetSupportRequests([FromQuery] RequestSearchCriteriaDto requestSearchCriteria)
    {
        var result = await _supportRequestService.GetRequestsByCriteriaAsync(requestSearchCriteria);
        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "WriteDataRole")]
    [HttpPost]
    [ProducesResponseType(typeof(Result<IEnumerable<SupportRequestDetailsDto>>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<IEnumerable<SupportRequestDetailsDto>>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateSupportRequest([FromBody] CreateSupportRequestDto createSupportRequestDto)
    {
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(creatorId))
        {
            return Unauthorized(Result<string>.Failure(new Error("UNAUTHORIZED", "User ID not found in token."), HttpStatusCode.Unauthorized));
        }

        var result = await _supportRequestService.CreateRequestAsync(createSupportRequestDto, creatorId);

        return result.Match<IActionResult>(
            onSuccess: success => CreatedAtAction(nameof(GetSupportRequestById), new { id = success.Data }, success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "WriteDataRole")]
    [HttpPost("comments")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AddComment([FromBody] CreateCommentDto createCommentDto)
    {
        var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(authorId))
        {
            return Unauthorized(Result<string>.Failure(new Error("UNAUTHORIZED", "User ID not found in token."), HttpStatusCode.Unauthorized));
        }

        var result = await _supportRequestService.AddCommentAsync(authorId, createCommentDto);

        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "WriteDataRole")]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateSupportRequest(Guid id, [FromBody] UpdateSupportRequestDto updateSupportRequestDto)
    {
        var result = await _supportRequestService.UpdateAsync(id, updateSupportRequestDto);

        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }

    [Authorize(Policy = "WriteDataRole")]
    [HttpPut("change-status/{id}")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] ChangeSupportRequestStatusDto changeSupportRequestStatusDto)
    {
        var modifiedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(modifiedByUserId))
        {
            return Unauthorized(Result<string>.Failure(new Error("UNAUTHORIZED", "User ID not found in token."), HttpStatusCode.Unauthorized));
        }

        var result = await _supportRequestService.ChangeStatusAsync(id, modifiedByUserId, changeSupportRequestStatusDto);

        return result.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => StatusCode(error.StatusCode, error)
        );
    }
}
