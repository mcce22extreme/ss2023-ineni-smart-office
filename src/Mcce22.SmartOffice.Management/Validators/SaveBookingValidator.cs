using FluentValidation;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Validators
{
    public class SaveBookingValidator : AbstractValidator<SaveBookingModel>
    {
        public SaveBookingValidator()
        {
            RuleFor(x => x.StartDateTime).LessThan(x => x.EndDateTime);

            RuleFor(x => x.UserId).GreaterThan(0);

            RuleFor(x => x.WorkspaceId).GreaterThan(0);
        }
    }
}
