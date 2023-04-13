using FluentValidation;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Workspaces.Validators
{
    public class SaveWorkspaceValidator : AbstractValidator<SaveWorkspaceModel>
    {
        public SaveWorkspaceValidator()
        {
            RuleFor(x => x.WorkspaceNumber).NotEmpty();
        }
    }
}
