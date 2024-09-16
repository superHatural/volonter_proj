using CSharpFunctionalExtensions;
using VolunteerProg.Domain.ValueObjects;

namespace VolunteerProg.Domain.Shared;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string? name = null)
        {
            var label = name ?? "value";
            return Error.Validation("value.is.invalid", $"{label} is invalid");
        }

        public static Error NotFound(Guid? id = null)
        {
            var forId = id == null ? "" : $"for id '{id}'";
            return Error.NotFound("record.is.invalid", $"record not found{forId}");
        }

        public static Error ValueIsRequired(string? name = null)
        {
            var label = name == null ? "" : " " + name + "";
            return Error.Validation("length.is.invalid", $"invalid{label}length");
        }
        public static Error NotFound(Phone? phone = null)
        {
            var phoneNumber = phone == null ? "" : $"for phone number '{phone.PhoneNumber}'";
            return Error.NotFound("record.is.invalid", $"record not found {phoneNumber}");
        }
        public static Error NotFound(Email? email = null)
        {
            var emailAddress = email == null ? "" : $"for email '{email.EmailAddress}'";
            return Error.NotFound("record.is.invalid", $"record not found {emailAddress}");
        }
        public static Error AlreadyExist()
        {
            return Error.Conflict("volunteer.already.exist", $"volunteer already exist");
        }
    }

}