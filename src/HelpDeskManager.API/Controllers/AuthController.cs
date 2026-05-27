using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskManager.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(Result<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<LoginResponseDto>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var userLogin = await _authService.LoginAsync(loginRequest);

        return userLogin.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => Unauthorized(error)
        );
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(Result<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<LoginResponseDto>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
    {
        var userRegister = await _authService.RegisterAsync(registerRequest);

        return userRegister.Match<IActionResult>(
            onSuccess: success => Ok(success),
            onFailure: error => Unauthorized(error)
        );
    }
}
