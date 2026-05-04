namespace PatientPortal.Tests;

using FluentAssertions;
using Moq;
using PatientPortal.Core.DTOs;
using PatientPortal.Core.Interfaces;
using PatientPortal.Core.Models;
using PatientPortal.Infrastructure.Services;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _repoMock;
    private readonly PatientService _service;

    public PatientServiceTests()
    {
        _repoMock = new Mock<IPatientRepository>();
        _service = new PatientService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenPatientDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync((Patient?)null);

        var result = await _service.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenDobIsInFuture()
    {
        var request = new CreatePatientRequest
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.UtcNow.AddDays(1),
            PhoneNumber = "555-1234",
            Email = "john@example.com"
        };

        var act = async () => await _service.CreateAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*past*");
    }

    [Fact]
    public async Task CreateAsync_ThrowsArgumentException_WhenDobIsTooOld()
    {
        var request = new CreatePatientRequest
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = DateTime.UtcNow.AddYears(-131),
            PhoneNumber = "555-1234",
            Email = "john@example.com"
        };

        var act = async () => await _service.CreateAsync(request);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*130 years*");
    }

    [Fact]
    public async Task CreateAsync_ReturnsPatientResponse_WhenInputIsValid()
    {
        var request = new CreatePatientRequest
        {
            FirstName = "Jane",
            LastName = "Smith",
            DateOfBirth = new DateTime(1990, 6, 15),
            PhoneNumber = "555-5678",
            Email = "jane@example.com"
        };

        _repoMock.Setup(r => r.CreateAsync(It.IsAny<Patient>()))
                 .ReturnsAsync((Patient p) => { p.Id = 1; return p; });

        var result = await _service.CreateAsync(request);

        result.Should().NotBeNull();
        result.FirstName.Should().Be("Jane");
        result.Id.Should().Be(1);
    }
}