using FluentValidation;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Workspaces.Validators
{
    public class SaveWorkspaceDataValidator : AbstractValidator<SaveWorkspaceDataModel>
    {
        public SaveWorkspaceDataValidator()
        {
            RuleFor(x => x.WorkspaceId).NotEmpty();

        }
    }
}
