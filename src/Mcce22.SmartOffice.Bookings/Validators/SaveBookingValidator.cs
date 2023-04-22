using FluentValidation;
using Mcce22.SmartOffice.Bookings.Models;

namespace Mcce22.SmartOffice.Bookings.Validators
{
    public class SaveBookingValidator : AbstractValidator<SaveBookingModel>
    {
        public SaveBookingValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.WorkspaceId).NotEmpty();

            RuleFor(x => x.Firstname).NotEmpty();

            RuleFor(x => x.Lastname).NotEmpty();

            RuleFor(x => x.Email).NotEmpty();

            RuleFor(x => x.WorkspaceNumber).NotEmpty();

            RuleFor(x => x.RoomNumber).NotEmpty();
        }
    }
}
