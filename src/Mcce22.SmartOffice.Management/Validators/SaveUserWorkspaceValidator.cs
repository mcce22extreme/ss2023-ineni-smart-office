using FluentValidation;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Validators
{
    public class SaveUserWorkspaceValidator : AbstractValidator<SaveUserWorkspaceModel>
    {
        public SaveUserWorkspaceValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);

            RuleFor(x => x.WorkspaceId).GreaterThan(0);

            RuleFor(x => x.DeskHeight).InclusiveBetween(70, 120);
        }
    }
}
