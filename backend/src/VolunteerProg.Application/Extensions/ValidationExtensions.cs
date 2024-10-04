using CSharpFunctionalExtensions;
using FluentValidation.Results;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Application.Extensions;

public static class ValidationExtensions
{
    public static ErrorList ToErrorList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let errorCode = Error.Deserialize(errorMessage)
            select Error.Validation(errorCode.Code, errorMessage, validationError.PropertyName);
        return errors.ToList(); 
    }
}