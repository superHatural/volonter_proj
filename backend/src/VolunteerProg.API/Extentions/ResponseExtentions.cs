using CSharpFunctionalExtensions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using VolunteerProg.API.Response;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.API.Extentions;

public static class ResponseExtentions
{
    public static ActionResult ToResponse(this Error error)
    {
        var statusCode = GetStatusCodeForErrorType(error.Type);
        var envelope = Envelope.Error(error.ToErrorList());
        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }

    public static ActionResult ToResponse(this ErrorList errors)
    {
        if (!errors.Any())
        {
            return new ObjectResult(null)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var distinctErrorsTypes = errors.Select(e => e.Type).Distinct().ToList();

        var statusCode = distinctErrorsTypes.Count() > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeForErrorType(distinctErrorsTypes.First());
        
        var envelope = Envelope.Error(errors);
        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }

    public static int GetStatusCodeForErrorType(this ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
}