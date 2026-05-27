using HelpDeskManager.BLL.Services;
using HelpDeskManager.Core.DTOs.Support;
using HelpDeskManager.Core.Entities;
using HelpDeskManager.Core.Enums;
using HelpDeskManager.Core.Interfaces;
using HelpDeskManager.Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System.Timers;

namespace HelpDeskManager.BLL.Tests;

public class SupportRequestServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ISupportRequestRepository> _mockSupportRepo;
    private readonly SupportRequestService _sut;

    public SupportRequestServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockSupportRepo = new Mock<ISupportRequestRepository>();

        _mockUnitOfWork.Setup(u => u.SupportRequestRepository).Returns(_mockSupportRepo.Object);

        var mockLogger = new Mock<ILogger<SupportRequestService>>();
        _sut = new SupportRequestService(_mockUnitOfWork.Object, mockLogger.Object);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenValidRequest_ShouldUpdateAndAddHistory()
    {
        // Arrange
        var requestId = Guid.NewGuid();
        var userId = "user-admin-123";

        var dto = new ChangeSupportRequestStatusDto
        {
            NewStatus = RequestStatus.Resolved,
            Notes = "Issue was fixed via remote session"
        };

        var existingRequest = new SupportRequest
        {
            Id = requestId,
            Status = RequestStatus.UnderReview,
            Subject = "Cannot connect to VPN",
            Description = "User reports that they cannot connect to the company VPN since yesterday.",
            CustomerId = Guid.NewGuid()
        };

        _mockSupportRepo
            .Setup(repo => repo.GetByIdAsync(requestId))
            .ReturnsAsync(existingRequest);

        _mockUnitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _sut.ChangeStatusAsync(requestId, userId, dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Status changed successfully.", result.Data);

        _mockSupportRepo.Verify(repo => repo.Update(It.Is<SupportRequest>(r =>
            r.Id == requestId && r.Status == RequestStatus.Resolved
        )), Times.Once);

        _mockSupportRepo.Verify(repo => repo.AddHistoryRecord(It.Is<RequestHistory>(h =>
            h.SupportRequestId == requestId &&
            h.PreviousStatus == RequestStatus.UnderReview &&
            h.NewStatus == RequestStatus.Resolved &&
            h.ChangeNotes == dto.Notes
        )), Times.Once);


        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenChangingStatusToAClosedRequest_ShouldReturnBadRequest()
    {
        // ARRANGE
        var requestId = Guid.NewGuid();
        var userId = "user-admin-123";

        var dto = new ChangeSupportRequestStatusDto { NewStatus = RequestStatus.UnderReview, Notes = "Review" };

        var existingRequest = new SupportRequest { Id = requestId, Status = RequestStatus.Closed };

        _mockSupportRepo.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(existingRequest);

        // ACT
        var result = await _sut.ChangeStatusAsync(requestId, userId, dto);

        // ASSERT
        Assert.False(result.IsSuccess);
        Assert.Equal("INVALID_REQUEST", result.Error?.Code);
        _mockSupportRepo.Verify(repo => repo.Update(It.IsAny<SupportRequest>()), Times.Never);
    }

    [Fact]
    public async Task ChangeStatusAsync_WhenClosingUnresolvedRequest_ShouldReturnBadRequest()
    {
        // ARRANGE
        var requestId = Guid.NewGuid();
        var userId = "user-admin-123";

        var dto = new ChangeSupportRequestStatusDto { NewStatus = RequestStatus.Closed, Notes = "Closing" };

        var existingRequest = new SupportRequest { Id = requestId, Status = RequestStatus.UnderReview };

        _mockSupportRepo.Setup(repo => repo.GetByIdAsync(requestId)).ReturnsAsync(existingRequest);

        // ACT
        var result = await _sut.ChangeStatusAsync(requestId, userId, dto);

        // ASSERT
        Assert.False(result.IsSuccess);
        Assert.Equal("INVALID_STATUS_TRANSITION", result.Error?.Code);
        _mockSupportRepo.Verify(repo => repo.Update(It.IsAny<SupportRequest>()), Times.Never);
    }
}
