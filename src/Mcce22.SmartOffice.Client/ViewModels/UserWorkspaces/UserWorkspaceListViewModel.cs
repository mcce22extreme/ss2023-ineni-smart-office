using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class UserWorkspaceListViewModel : ListViewModelBase<UserWorkspaceModel>
    {
        private readonly IUserWorkspaceManager _userWorkspaceManager;
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IUserManager _userManager;

        public UserWorkspaceListViewModel(
            IUserWorkspaceManager userWorkspaceManager,
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
            await DialogService.ShowDialog(new UserWorkspaceDetailViewModel(_userWorkspaceManager, _workspaceManager, _userManager, DialogService));
        }

        protected override async Task OnEdit()
        {
            await DialogService.ShowDialog(new UserWorkspaceDetailViewModel(SelectedItem, _userWorkspaceManager, _workspaceManager, _userManager, DialogService));
        }

        protected override async Task OnDelete()
        {
            await _userManager.Delete(SelectedItem.Id);
        }

        protected override async Task<UserWorkspaceModel[]> OnReload()
        {
            return await _userWorkspaceManager.GetList();
        }
    }
}
