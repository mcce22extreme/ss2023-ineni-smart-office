using System.Linq;
using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class WorkspaceDataListViewModel : ListViewModelBase<WorkspaceDataModel>
    {
        private readonly IWorkspaceDataManager _workspaceDataManager;

        public WorkspaceDataListViewModel(IWorkspaceDataManager workspaceDataManager, IDialogService dialogService)
            : base(dialogService)
        {
            _workspaceDataManager = workspaceDataManager;
        }

        protected override async Task<WorkspaceDataModel[]> OnReload()
        {
            var data = await _workspaceDataManager.GetList();
            return data.OrderByDescending(x => x.Timestamp).ToArray();
        }

        protected override bool CanDelete()
        {
            return !IsBusy;
        }

        protected override async void Delete()
        {
            if (CanDelete())
            {
                try
                {
                    IsBusy = true;

                    var confirmDelete = new ConfirmDeleteViewModel("Delete all entries?", "Do you really want to delete all entries?", DialogService);

                    await DialogService.ShowDialog(confirmDelete);

                    if (confirmDelete.Confirmed)
                    {
                        await OnDelete();
                    }
                }
                finally
                {
                    IsBusy = false;
                }

                Reload();
            }
        }

        protected override async Task OnDelete()
        {
            await _workspaceDataManager.DeleteAll();
        }
    }
}
