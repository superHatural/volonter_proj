using System.Data;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VolunteerProg.Application.Database;
using VolunteerProg.Application.FileProvider;
using VolunteerProg.Application.Providers;
using VolunteerProg.Application.Volunteer;
using VolunteerProg.Application.Volunteer.PetCreate.AddFile;
using VolunteerProg.Domain.Aggregates.PetManagement.Entities;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace Volunteer.Application.Tests;

public class AddFileHandlerTests
{
    private readonly Mock<IFileProvider> _fileProviderMock;
    private readonly Mock<ILogger<AddFileHandler>> _loggerMock;
    private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AddFileHandler _handler;

    public AddFileHandlerTests()
    {
        _fileProviderMock = new Mock<IFileProvider>();
        _loggerMock = new Mock<ILogger<AddFileHandler>>();
        _volunteersRepositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new AddFileHandler(
            _fileProviderMock.Object,
            _loggerMock.Object,
            _volunteersRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_Volunteer_Not_Found()
    {
        // Arrange
        var command = new AddFileCommand
        (
            new List<CreateFileData>
            {
                new CreateFileData(new MemoryStream(), "testfile.txt")
            },
            Guid.NewGuid(),
            Guid.NewGuid());


        // Настройка моков для возврата ошибки при запросе волонтера
        _volunteersRepositoryMock
            .Setup(x => 
                x.GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot.Volunteer, Error>(Errors.General.NotFound(Guid.Empty)));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_FilePath_Creation_Fails()
    {
        // Arrange
        var command = new AddFileCommand
        (
            new List<CreateFileData>
            {
                new CreateFileData(new MemoryStream(), "testfile.txt")
            },
            Guid.NewGuid(),
            Guid.NewGuid());

        // Мок успешного возврата волонтера
        _volunteersRepositoryMock
            .Setup(x => 
                x.GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot.Volunteer, Error>(CreateVolunteer()));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_File_Uploaded_Successfully()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet = CreatePet();
        volunteer.AddPet(pet);
  
        var command = new AddFileCommand
        (
            new List<CreateFileData>
            {
                new (new MemoryStream(), "testfile.jpg")
            },
            volunteer.Id.Value,
            volunteer.Pets.FirstOrDefault(v => v.Id == pet.Id)!.Id.Value);
        
        // Мок успешного возврата волонтера
        _volunteersRepositoryMock
            .Setup(x =>
                x.GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot.Volunteer, Error>(volunteer));

        // Мок успешного сохранения файлов
        _fileProviderMock
            .Setup(x => 
                x.UploadFiles(It.IsAny<List<FileData>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<IReadOnlyList<FilePath>, Error>(new List<FilePath>
                { FilePath.Create(NotEmptyVo.Create("path").Value, ".jpg").Value }));

        // Мок успешного сохранения изменений
        _unitOfWorkMock
            .Setup(x => x.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Rollback_Transaction_On_Failure()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet = CreatePet();
        volunteer.AddPet(pet);

        var command = new AddFileCommand
        (
            new List<CreateFileData>
            {
                new (new MemoryStream(), "testfile.jpg")
            },
            volunteer.Id.Value,
            volunteer.Pets.FirstOrDefault(v => v.Id == pet.Id)!.Id.Value);
        
        // Мок успешного возврата волонтера
        _volunteersRepositoryMock
            .Setup(x => 
                x.GetById(It.IsAny<VolunteerId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot.Volunteer, Error>(volunteer));

        // Мок ошибки при загрузке файлов
        _fileProviderMock
            .Setup(x => 
                x.UploadFiles(It.IsAny<List<FileData>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<IReadOnlyList<FilePath>, Error>(Error.Failure("file.upload", "Fail to upload files in minio")));

        var transactionMock = new Mock<IDbTransaction>();

        // Мок транзакции
        _unitOfWorkMock
            .Setup(x => x.BeginTransaction(It.IsAny<CancellationToken>()))
            .ReturnsAsync(transactionMock.Object);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
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