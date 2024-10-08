using FluentValidation;
using VolunteerProg.Application.Validation;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

namespace VolunteerProg.Application.Volunteer.Update.UpdateRequisites;

public class UpdateVolunteerRequisiteDtoValidation : AbstractValidator<UpdateVolunteerRequisitesDto>
{
    public UpdateVolunteerRequisiteDtoValidation()
    {
        RuleForEach(c => c.RequisitesRecords)
            .MustBeValueObject(x => Requisite.Create(x.Title, x.Description));
    }
}