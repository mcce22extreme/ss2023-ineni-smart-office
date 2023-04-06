using FluentValidation;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Validators
{
    public class SaveSlideshowItemValidator : AbstractValidator<SaveSlideshowItemModel>
    {
        public SaveSlideshowItemValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);

            RuleFor(x => x.FileName).NotEmpty();
        }
    }
}
