using HelpDeskManager.BLL.Services;
using HelpDeskManager.Core.DTOs.Account;
using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Repositories;
using HelpDeskManager.Core.Interfaces.Services;
using HelpDeskManager.DAL.Data.Identity;
using Moq;
using System.Net;

namespace HelpDeskManager.BLL.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IUserRepository> _mockUserRepo;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTokenService = new Mock<ITokenService>();
        _mockUserRepo = new Mock<IUserRepository>();

        _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepo.Object);

        _sut = new AuthService(_mockUnitOfWork.Object, _mockTokenService.Object);
    }

    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Email = "ghost@helpdesk.com", Password = "AnyPassword!" };

        _mockUserRepo
            .Setup(repo => repo.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync((AppUserDto?)null);

        //Act
        var result = await _sut.LoginAsync(loginDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
        _mockTokenService.Verify(t => t.CreateToken(It.IsAny<AppUserDto>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginDto = new LoginRequestDto { Email = "admin@test.com", Password = "WrongPa$$w0rd" };

        var existingUser = new AppUserDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = loginDto.Email
        };

        _mockUserRepo
            .Setup(repo => repo.GetByEmailAsync(loginDto.Email))
            .ReturnsAsync(existingUser);

        //Act
        var result = await _sut.LoginAsync(loginDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
        _mockTokenService.Verify(t => t.CreateToken(It.IsAny<AppUserDto>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ShouldReturnToken()
    {
        // Arggande
        var loginRequest = new LoginRequestDto
        {
            Email = "admin@test.com",
            Password = "Pa$$w0rd"
        };

        var expectedUser = new AppUserDto
        {
            Id = Guid.NewGuid().ToString(),
            Email = loginRequest.Email,
        };

        var expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";

        _mockUserRepo
            .Setup(repo => repo.ValidateCredentialsAsync(loginRequest.Email, loginRequest.Password))
            .ReturnsAsync((true, expectedUser));

        _mockTokenService
            .Setup(t => t.CreateToken(expectedUser))
            .ReturnsAsync(expectedToken);


        // Act
        var result = await _sut.LoginAsync(loginRequest);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

        Assert.NotNull(result.Data);
        Assert.Equal(expectedToken, result.Data.AccessToken);

        _mockTokenService.Verify(t => t.CreateToken(expectedUser), Times.Once());
    }
}
