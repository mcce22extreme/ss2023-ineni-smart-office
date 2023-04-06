using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;
using Newtonsoft.Json;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class SeedDataViewModel : ViewModelBase
    {
        private readonly IUserManager _userManager;
        private readonly IWorkspaceManager _workspaceManager;
        private readonly ISlideshowItemManager _slideshowItemManager;
        private readonly IDialogService _dialogService;

        public RelayCommand SeedAllCommand { get; }

        public RelayCommand DeleteAllCommand { get; }

        public SeedDataViewModel(
            IUserManager userManager,
            IWorkspaceManager workspaceManager,
            ISlideshowItemManager slideshowItemManager,
            IDialogService dialogService)
        {
            _userManager = userManager;
            _workspaceManager = workspaceManager;
            _slideshowItemManager = slideshowItemManager;
            _dialogService = dialogService;

            SeedAllCommand = new RelayCommand(SeedAll, CanSeed);
            DeleteAllCommand = new RelayCommand(DeleteAll, CanDelete);
        }

        protected override void UpdateCommandStates()
        {
            base.UpdateCommandStates();

            SeedAllCommand.NotifyCanExecuteChanged();
            DeleteAllCommand.NotifyCanExecuteChanged();
        }

        private bool CanSeed()
        {
            return !IsBusy;
        }

        private async void SeedAll()
        {
            try
            {
                IsBusy = true;

                await SeedUsers();
                await SeedWorkspaces();
                await SeedSlideshowItems();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SeedUsers()
        {
            var json = File.ReadAllText("seeddata\\users.json");
            var users = JsonConvert.DeserializeObject<UserModel[]>(json);

            foreach (var user in users)
            {
                await _userManager.Save(user);
            }
        }

        private async Task SeedWorkspaces()
        {
            var json = File.ReadAllText("seeddata\\workspaces.json");
            var workspaces = JsonConvert.DeserializeObject<WorkspaceModel[]>(json);

            foreach (var workspace in workspaces)
            {
                await _workspaceManager.Save(workspace);
            }
        }

        private async Task SeedSlideshowItems()
        {
            var users = await _userManager.GetList();
            var user = users.FirstOrDefault();

            var filePaths = Directory.GetFiles("sampleimages");
            foreach (var filePath in filePaths)
            {
                var item = await _slideshowItemManager.Save(new SlideshowItemModel
                {
                    FileName = Path.GetFileName(filePath),
                    UserId = user.Id
                });

                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                await _slideshowItemManager.StoreContent(item.Id, fs);
            }
        }

        public bool CanDelete()
        {
            return !IsBusy;
        }

        private async void DeleteAll()
        {
            try
            {
                IsBusy = true;

                if (await ConfirmDelete("users, workspaces and slideshowitems"))
                {
                    await DeleteSlideshowItems();
                    await DeleteUsers();
                    await DeleteWorkspaces();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteUsers()
        {
            var users = await _userManager.GetList();

            foreach (var user in users)
            {
                await _userManager.Delete(user.Id);
            }
        }

        private async Task DeleteWorkspaces()
        {
            var workspaces = await _workspaceManager.GetList();

            foreach (var workspace in workspaces)
            {
                await _workspaceManager.Delete(workspace.Id);
            }
        }

        private async Task DeleteSlideshowItems()
        {
            var slideshowItems = await _slideshowItemManager.GetList();

            foreach (var item in slideshowItems)
            {
                await _slideshowItemManager.Delete(item.Id);
            }
        }

        private async Task<bool> ConfirmDelete(string type)
        {
            var confirmDelete = new ConfirmDeleteViewModel("Delete items...", $"Are you sure you want delete all {type}?", _dialogService);

            await _dialogService.ShowDialog(confirmDelete);

            return confirmDelete.Confirmed;
        }
    }
}
