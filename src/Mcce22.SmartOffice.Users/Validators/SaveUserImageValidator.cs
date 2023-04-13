using FluentValidation;
using Mcce22.SmartOffice.Users.Models;

namespace Mcce22.SmartOffice.Users.Validators
{
    public class SaveUserImageValidator : AbstractValidator<SaveUserImageModel>
    {
        public SaveUserImageValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.FileName).NotEmpty();
        }
    }
}
