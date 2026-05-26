using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Services;
using HelpDeskManager.DAL.Mappers;
using System.Net;

namespace HelpDeskManager.BLL.Services;

public class AuthService : IAuthService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }



    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
    {
        var (IsValid, User) = await _unitOfWork.UserRepository.ValidateCredentialsAsync(loginRequestDto.Email, loginRequestDto.Password);

        if (!IsValid || User == null)
        {
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
            return Result<LoginResponseDto>.Failure(
                new Error("CREATE_USER_FAILED", $"Failed to create userDto: {errorMessages}"));
        }

        var token = await _tokenService.CreateToken(userDto);

        var loginResponse = new LoginResponseDto { AccessToken = token };

        return Result<LoginResponseDto>.Success(loginResponse);
    }
}
