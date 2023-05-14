using FluentValidation;
using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Validators
{
    public class SaveUserValidator : AbstractValidator<SaveUserModel>
    {
        public SaveUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();

            RuleFor(x => x.FirstName).NotEmpty();

            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
