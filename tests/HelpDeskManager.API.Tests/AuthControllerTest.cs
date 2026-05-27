using HelpDeskManager.API.Controllers;
using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.DTOs.Results;
using HelpDeskManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace CustomerManager.API.Tests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly AuthController _sut;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();

        _sut = new AuthController(_mockAuthService.Object);
    }

    [Fact]
    public async Task Login_WhenCredentialsAreInvalid_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequestDto { Email = "admin@test.com", Password = "Wrong" };

        var failedResult = Result<LoginResponseDto>.Failure(
            new Error("INVALID_CREDENTIALS", "Invalid email or password."),
            HttpStatusCode.Unauthorized
        );

        _mockAuthService
            .Setup(s => s.LoginAsync(loginRequest))
            .ReturnsAsync(failedResult);

        var actionResult = await _sut.Login(loginRequest);

        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(actionResult);

        var resultWrapper = Assert.IsType<Result<LoginResponseDto>>(unauthorizedResult.Value);

        Assert.NotNull(resultWrapper.Error);
        Assert.Equal("INVALID_CREDENTIALS", resultWrapper.Error.Code);
    }
}