using System;
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
        private static readonly Random Random = new Random();

        private readonly IUserManager _userManager;
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IUserImageManager _userImageManager;
        private readonly IWorkspaceDataManager _workspaceDataManager;
        private readonly IDialogService _dialogService;

        public RelayCommand SeedDataCommand { get; }

        public RelayCommand DeleteDataCommand { get; }

        public SeedDataViewModel(
            IUserManager userManager,
            IWorkspaceManager workspaceManager,
            IUserImageManager userImageManager,
            IWorkspaceDataManager workspaceDataManager,
            IDialogService dialogService)
        {
            _userManager = userManager;
            _workspaceManager = workspaceManager;
            _userImageManager = userImageManager;
            _workspaceDataManager = workspaceDataManager;
            _dialogService = dialogService;

            SeedDataCommand = new RelayCommand(SeedData, CanSeed);
            DeleteDataCommand = new RelayCommand(DeleteData, CanDelete);
        }

        protected override void UpdateCommandStates()
        {
            base.UpdateCommandStates();

            SeedDataCommand.NotifyCanExecuteChanged();
            DeleteDataCommand.NotifyCanExecuteChanged();
        }

        private bool CanSeed()
        {
            return !IsBusy;
        }

        private async void SeedData()
        {
            try
            {
                IsBusy = true;

                await SeedUsers();
                await SeedWorkspaces();
                await SeedUserImages();
                await SeedWorkspaceData();
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

        private async Task SeedUserImages()
        {
            var users = await _userManager.GetList();
            var user = users.FirstOrDefault();

            var filePaths = Directory.GetFiles("sampleimages");
            foreach (var filePath in filePaths)
            {
                var item = await _userImageManager.Save(new UserImageModel
                {
                    FileName = Path.GetFileName(filePath),
                    UserId = user.Id
                });

                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                await _userImageManager.StoreContent(item.Id, fs);
            }
        }

        private async Task SeedWorkspaceData()
        {
            for (int i = 1; i <= 30; i++)
            {
                for (int j = 1; j < 24; j++)
                {
                    for (int k = 1; k < 4; k++)
                    {
                        var model = new WorkspaceDataModel
                        {
                            WorkspaceId = 1,
                            Timestamp = new DateTime(2023,03,i, j, k*15, 0),
                            Temperature = Random.Next(15, 25),
                            Noise = Random.Next(60, 70),
                            Humidity= Random.Next(40, 60),
                            Co2 = Random.Next(400, 999),
                            Luminosity = Random.Next(100, 400)
                        };
                        await _workspaceDataManager.Save(model);
                    }                    
                }
            }
        }

        public bool CanDelete()
        {
            return !IsBusy;
        }

        private async void DeleteData()
        {
            try
            {
                IsBusy = true;

                if (await ConfirmDelete("users, user images, workspaces and workspace data"))
                {
                    await DeleteUserImages();
                    await DeleteUsers();
                    await DeleteWorkspaces();
                    await DeleteWorkspaceData();
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

        private async Task DeleteUserImages()
        {
            var userImages = await _userImageManager.GetList();

            foreach (var item in userImages)
            {
                await _userImageManager.Delete(item.Id);
            }
        }

        private async Task DeleteWorkspaceData()
        {
            var workspaceData = await _workspaceDataManager.GetList();

            foreach (var data in workspaceData)
            {
                await _workspaceDataManager.Delete(data.Id);
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
