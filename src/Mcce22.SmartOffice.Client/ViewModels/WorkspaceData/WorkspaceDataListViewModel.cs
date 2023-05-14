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
            return await _workspaceDataManager.GetList();
        }

        protected override async Task OnDelete()
        {
            await _workspaceDataManager.Delete(SelectedItem.Id);
        }
    }
}
