using FluentValidation;
using Mcce22.SmartOffice.Workspaces.Models;

namespace Mcce22.SmartOffice.Workspaces.Validators
{
    public class SaveWorkspaceConfigurationValidator : AbstractValidator<SaveWorkspaceConfigurationModel>
    {
        public SaveWorkspaceConfigurationValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.WorkspaceId).NotEmpty();

            RuleFor(x => x.DeskHeight).InclusiveBetween(70, 120);
        }
    }
}
