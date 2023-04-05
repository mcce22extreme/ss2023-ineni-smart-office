using FluentValidation;
using Mcce22.SmartOffice.Management.Models;

namespace Mcce22.SmartOffice.Management.Validators
{
    public class SaveWorkspaceValidator : AbstractValidator<SaveWorkspaceModel>
    {
        public SaveWorkspaceValidator()
        {
            RuleFor(x => x.WorkspaceNumber).NotEmpty();
        }
    }
}
