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
        }
    }
}
