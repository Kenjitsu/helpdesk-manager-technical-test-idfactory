using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Services;
using HelpDeskManager.DAL.Mappers;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HelpDeskManager.BLL.Services;

public class AuthService : IAuthService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _logger = logger;
    }



    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var (IsValid, User) = await _unitOfWork.UserRepository.ValidateCredentialsAsync(loginRequestDto.Email, loginRequestDto.Password);

        if (!IsValid || User == null)
        {
            _logger.LogWarning("Failed login attempt for email: {Email}", loginRequestDto.Email);
            return Result<LoginResponseDto>.Failure(
                new Error("INVALID_CREDENTIALS", "Invalid email or password."),
                HttpStatusCode.Unauthorized
            );
        }

        var token = await _tokenService.CreateToken(User);

        var loginResponse = new LoginResponseDto { AccessToken = token };

        return Result<LoginResponseDto>.Success(loginResponse);
    }

    public async Task<Result<LoginResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto)
    {
        var userDto = registerRequestDto.ToAppUserDto();

        var (Success, Errors) = await _unitOfWork.UserRepository.CreateUserAsync(registerRequestDto.Email, registerRequestDto.Password, userDto);

        if (!Success)
        {
            var errorMessages = string.Join(", ", Errors);
            _logger.LogError("Failed to create user: {Errors}", errorMessages);
            return Result<LoginResponseDto>.Failure(
                new Error("CREATE_USER_FAILED", $"Failed to create userDto: {errorMessages}"), HttpStatusCode.NotFound);
        }

        var token = await _tokenService.CreateToken(userDto);

        var loginResponse = new LoginResponseDto { AccessToken = token };

        return Result<LoginResponseDto>.Success(loginResponse);
    }
}
