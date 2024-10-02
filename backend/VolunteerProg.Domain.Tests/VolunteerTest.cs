using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot;
using VolunteerProg.Domain.Aggregates.PetManagement.Entities;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

public class VolunteerTests
{
    [Fact]
    public void AddPet_Should_AddPetWithCorrectSerialNumber()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet = CreatePet();

        // Act
        var result = volunteer.AddPet(pet);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Count.Should().Be(1);
        volunteer.Pets[0].SerialNumber.Value.Should().Be(1);
    }

    [Fact]
    public void ChangePetPosition_Should_ChangePositionCorrectly()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);

        // Act
        var result = volunteer.ChangePetPosition(pet1.Id, SerialNumber.Create(2).Value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets[0].SerialNumber.Value.Should().Be(2);
        volunteer.Pets[1].SerialNumber.Value.Should().Be(1);
    }

    [Fact]
    public void ChangePetPosition_Should_MovePetDownInTheList()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        var pet3 = CreatePet();
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);

        // Act
        var result = volunteer.ChangePetPosition(pet1.Id, SerialNumber.Create(3).Value);

        // Assert
        result.IsSuccess.Should().BeTrue();

        volunteer.Pets[0].SerialNumber.Value.Should().Be(3); // Питомец 1 перемещен на третью позицию
        volunteer.Pets[1].SerialNumber.Value.Should().Be(1); // Питомец 2 на первой позиции
        volunteer.Pets[2].SerialNumber.Value.Should().Be(2); // Питомец 3 на второй позиции
    }

    [Fact]
    public void ChangePetPosition_Should_MovePetUpInTheList()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        var pet3 = CreatePet();
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);
        volunteer.AddPet(pet3);

        // Act
        var result = volunteer.ChangePetPosition(pet3.Id, SerialNumber.Create(1).Value);

        // Assert
        result.IsSuccess.Should().BeTrue();

        volunteer.Pets[0].SerialNumber.Value.Should().Be(2); // Питомец 1 перемещен на вторую позицию
        volunteer.Pets[1].SerialNumber.Value.Should().Be(3); // Питомец 2 на третьей позиции
        volunteer.Pets[2].SerialNumber.Value.Should().Be(1); // Питомец 3 перемещен на первую позицию
    }

    [Fact]
    public void ChangePetPosition_Should_NotChangePosition_IfNewPositionIsSame()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);

        // Act
        var result =
            volunteer.ChangePetPosition(pet1.Id, SerialNumber.Create(1).Value); // Пытаемся установить тот же номер

        // Assert
        result.IsSuccess.Should().BeTrue();

        volunteer.Pets[0].SerialNumber.Value.Should().Be(1); // Позиция не изменилась
        volunteer.Pets[1].SerialNumber.Value.Should().Be(2); // Вторая позиция также не изменилась
    }

    [Fact]
    public void ChangePetPosition_Should_ReturnNotFoundError_IfPetDoesNotExist()
    {
        // Arrange
        var volunteer = CreateVolunteer();
        var pet1 = CreatePet();
        var pet2 = CreatePet();
        volunteer.AddPet(pet1);
        volunteer.AddPet(pet2);

        var nonExistentPetId = PetId.Empty(); // Не существующий ID питомца

        // Act
        var result = volunteer.ChangePetPosition(nonExistentPetId, SerialNumber.Create(1).Value);

        // Assert
        result.IsFailure.Should().BeTrue(); // Должен вернуть ошибку
        result.Error.Should().Be(Errors.General.NotFound(nonExistentPetId)); // Ошибка "Не найдено"
    }

    private Volunteer CreateVolunteer()
    {
        var notEmptyString = NotEmptyVo.Create("test").Value;
        return new Volunteer(
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