using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class WorkspaceConfigurationListViewModel : ListViewModelBase<WorkspaceConfigurationModel>
    {
        private readonly IWorkspaceConfigurationManager _userWorkspaceManager;
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IUserManager _userManager;

        public WorkspaceConfigurationListViewModel(
            IWorkspaceConfigurationManager userWorkspaceManager,
            IWorkspaceManager workspaceManager,
            IUserManager userManager,
            IDialogService dialogService)
            : base(dialogService)
        {
            _userWorkspaceManager = userWorkspaceManager;
            _workspaceManager = workspaceManager;
            _userManager = userManager;
        }

        protected override async Task OnAdd()
        {
            await DialogService.ShowDialog(new WorkspaceConfigurationDetailViewModel(_userWorkspaceManager, _workspaceManager, _userManager, DialogService));
        }

        protected override async Task OnEdit()
        {
            await DialogService.ShowDialog(new WorkspaceConfigurationDetailViewModel(SelectedItem, _userWorkspaceManager, _workspaceManager, _userManager, DialogService));
        }

        protected override async Task OnDelete()
        {
            await _userWorkspaceManager.Delete(SelectedItem.Id);
        }

        protected override async Task<WorkspaceConfigurationModel[]> OnReload()
        {
            return await _userWorkspaceManager.GetList();
        }
    }
}
