using System.Data;
using System.Globalization;
using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using VolunteerProg.Application.Database;
using VolunteerProg.Application.Volunteer;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Application.Volunteer.PetCreate.Create;
using VolunteerProg.Domain.Aggregates.PetManagement.Entities;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace Volunteer.Application.Tests;

public class CreatePetHandlerTests
{
    private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock;
    private readonly Mock<ILogger<CreatePetHandler>> _loggerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<CreatePetCommand>> _validatorMock;
    private readonly CreatePetHandler _handler;

    public CreatePetHandlerTests()
    {
        _volunteersRepositoryMock = new Mock<IVolunteersRepository>();
        _loggerMock = new Mock<ILogger<CreatePetHandler>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<CreatePetCommand>>();

        _handler = new CreatePetHandler(
            _volunteersRepositoryMock.Object,
            _loggerMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Valid_Command()
    {
        // Arrange
        var command = new CreatePetCommand
        (
            Guid.NewGuid(),
            "PetName",
            "PetDescription",
            Guid.Empty,
            Guid.Empty,
            "Black",
            "Healthy",
            new AddressDto("City", "Country", "12345", "Street"),
            10,
            50,
            "89111147495",
            true,
            DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
            true,
            "NeedsHelp",
            []
        );

        _validatorMock
            .Setup(v =>
                v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _volunteersRepositoryMock
            .Setup(v => 
                v.GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot.Volunteer, Error>(CreateVolunteer()));

        _unitOfWorkMock.Setup(u => 
                u.BeginTransaction(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDbTransaction>());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        _unitOfWorkMock.Verify(u => 
            u.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_Validation_Fails()
    {
        // Arrange
        var command = new CreatePetCommand
        (Guid.NewGuid(),
            "PetTest",
            "PetDescription",
            Guid.Empty,
            Guid.Empty,
            "Black",
            "Healthy",
            new AddressDto("City", "Country", "12345", "Street"),
            10,
            50,
            "89111147495",
            true,
            DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
            true,
            "NeedsHelp",
            []);


        var validationFailures = new List<FluentValidation.Results.ValidationFailure>
        {
            new(nameof(command.Name), "Name is required.")
        };

        _validatorMock
            .Setup(v =>
                v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult(validationFailures));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        _unitOfWorkMock.Verify(u => 
            u.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Rollback_Transaction_On_Exception()
    {
        // Arrange
        var command = new CreatePetCommand
        (Guid.NewGuid(),
            "PetName",
            "PetDescription",
            Guid.Empty,
            Guid.Empty,
            "Black",
            "Healthy",
            new AddressDto("City", "Country", "12345", "Street"),
            10,
            50,
            "89111147495",
            true,
            DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
            true,
            "NeedsHelp",
            []);

        _validatorMock
            .Setup(v =>
                v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _volunteersRepositoryMock
            .Setup(v =>
                v.GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        var transactionMock = new Mock<IDbTransaction>();
        _unitOfWorkMock
            .Setup(u =>
                u.BeginTransaction(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        transactionMock.Verify(t => t.Rollback(), Times.Once);
        _unitOfWorkMock.Verify(u =>
            u.SaveChanges(It.IsAny<CancellationToken>()), Times.Never);
    }

    private VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot.Volunteer CreateVolunteer()
    {
        var notEmptyString = NotEmptyVo.Create("test").Value;
        return new VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot.Volunteer(
            VolunteerId.NewVolunteerId(),
            FullName.Create("test", "test").Value,
            Email.Create("john.doe@example.com").Value,
            notEmptyString,
            20,
            Phone.Create("89111147495").Value,
            null,
            null
        );
    }

    private Pet CreatePet()
    {
        var notEmptyString = NotEmptyVo.Create("test").Value;
        return new Pet(
            PetId.NewPetId(),
            notEmptyString,
            notEmptyString,
            SpeciesDetails.Create(SpeciesId.Empty(), BreedId.Empty()).Value,
            notEmptyString,
            notEmptyString,
            Address.Create("test", "test", "test", "test").Value,
            5,
            20,
            Phone.Create("89111147495").Value,
            true,
            Date.Create("2022-01-01").Value,
            true,
            PetStatus.NeedsHelp,
            null,
            null
        );
    }
}