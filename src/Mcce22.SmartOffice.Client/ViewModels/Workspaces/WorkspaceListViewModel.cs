using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public partial class WorkspaceListViewModel : ListViewModelBase<WorkspaceModel>
    {
        private readonly IWorkspaceManager _workspaceManager;

        public WorkspaceListViewModel(IWorkspaceManager workspaceManager, IDialogService dialogService)
            : base(dialogService)
        {
            _workspaceManager = workspaceManager;
        }

        protected override void UpdateCommandStates()
        {
            base.UpdateCommandStates();

            CopyToClipboardCommand.NotifyCanExecuteChanged();
        }

        protected override async Task OnAdd()
        {
            await DialogService.ShowDialog(new WorkspaceDetailViewModel(_workspaceManager, DialogService));
        }

        protected override async Task OnEdit()
        {
            await DialogService.ShowDialog(new WorkspaceDetailViewModel(SelectedItem, _workspaceManager, DialogService));
        }

        protected override async Task OnDelete()
        {
            await _workspaceManager.Delete(SelectedItem.Id);
        }

        protected override async Task<WorkspaceModel[]> OnReload()
        {
            return await _workspaceManager.GetList();
        }

        [RelayCommand(CanExecute = nameof(CanCopyToClipboard))]
        public void CopyToClipboard()
        {
            if (CanCopyToClipboard())
            {
                Clipboard.SetDataObject(SelectedItem.Id);
            }
        }

        public bool CanCopyToClipboard()
        {
            return !IsBusy && SelectedItem != null;
        }
    }
}
