using FluentValidation;
using Mcce22.SmartOffice.Bookings.Models;

namespace Mcce22.SmartOffice.Bookings.Validators
{
    public class SaveBookingValidator : AbstractValidator<SaveBookingModel>
    {
        public SaveBookingValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);

            RuleFor(x => x.WorkspaceId).GreaterThan(0);
        }
    }
}
