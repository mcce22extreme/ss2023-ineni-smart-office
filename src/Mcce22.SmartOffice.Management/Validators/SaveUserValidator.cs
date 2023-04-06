using FluentValidation;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Validators
{
    public class SaveUserValidator : AbstractValidator<SaveUserModel>
    {
        public SaveUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();

            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
