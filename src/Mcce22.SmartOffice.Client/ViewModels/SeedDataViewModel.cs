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
        private readonly IBookingManager _bookingManager;
        private readonly IWorkspaceDataManager _workspaceDataManager;
        private readonly IDialogService _dialogService;

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        private int _stepProgress;
        public int StepProgress
        {
            get { return _stepProgress; }
            set { SetProperty(ref _stepProgress, value); }
        }

        private string _progressText;
        public string ProgressText
        {
            get { return _progressText; }
            set { SetProperty(ref _progressText, value); }
        }

        private bool _activateUserSeed = true;
        public bool ActivateUserSeed
        {
            get { return _activateUserSeed; }
            set { SetProperty(ref _activateUserSeed, value); }
        }

        private bool _activateUserImageSeed = true;
        public bool ActivateUserImageSeed
        {
            get { return _activateUserImageSeed; }
            set { SetProperty(ref _activateUserImageSeed, value); }
        }

        private bool _activateWorkspaceSeed = true;
        public bool ActivateWorkspaceSeed
        {
            get { return _activateWorkspaceSeed; }
            set { SetProperty(ref _activateWorkspaceSeed, value); }
        }

        private bool _activateWorkspaceConfigSeed = true;
        public bool ActivateWorkspaceConfigSeed
        {
            get { return _activateWorkspaceSeed; }
            set { SetProperty(ref _activateWorkspaceConfigSeed, value); }
        }

        private bool _activateWorkspaceDataSeed = false;
        public bool ActivateUserDataSeed
        {
            get { return _activateWorkspaceDataSeed; }
            set { SetProperty(ref _activateWorkspaceDataSeed, value); }
        }

        private bool _activateBookingSeed = true;
        public bool ActivateBookingSeed
        {
            get { return _activateBookingSeed; }
            set { SetProperty(ref _activateBookingSeed, value); }
        }

        public RelayCommand SeedDataCommand { get; }

        public SeedDataViewModel(
            IUserManager userManager,
            IWorkspaceManager workspaceManager,
            IUserImageManager userImageManager,
            IBookingManager bookingManager,
            IWorkspaceDataManager workspaceDataManager,
            IDialogService dialogService)
        {
            _userManager = userManager;
            _workspaceManager = workspaceManager;
            _userImageManager = userImageManager;
            _bookingManager = bookingManager;
            _workspaceDataManager = workspaceDataManager;
            _dialogService = dialogService;

            SeedDataCommand = new RelayCommand(SeedData, CanSeed);
        }

        protected override void UpdateCommandStates()
        {
            base.UpdateCommandStates();

            SeedDataCommand.NotifyCanExecuteChanged();
        }

        private bool CanSeed()
        {
            return !IsBusy;
        }

        private async void SeedData()
        {
            try
            {
                var confirmDelete = new ConfirmDeleteViewModel("Caution!", $"This operation will erase all existing data. Are you sure you want to continue?", _dialogService);

                await _dialogService.ShowDialog(confirmDelete);

                if (confirmDelete.Confirmed)
                {
                    IsBusy = true;

                    Progress = 0;
                    if (ActivateUserImageSeed)
                    {
                        ProgressText = "Delete user images...";
                        await DeleteUserImages();
                    }

                    Progress = 10;
                    if (ActivateUserSeed)
                    {
                        ProgressText = "Delete users...";
                        await DeleteUsers();
                    }


                    Progress = 20;
                    if (ActivateWorkspaceSeed)
                    {
                        ProgressText = "Delete workspaces...";
                        await DeleteWorkspaces();
                    }

                    Progress = 40;
                    if (ActivateUserDataSeed)
                    {
                        ProgressText = "Delete workspace data...";
                        await DeleteWorkspaceData();
                    }

                    Progress = 45;
                    if (ActivateBookingSeed)
                    {
                        ProgressText = "Delete booking data...";
                        await DeleteBookings();
                    }

                    Progress = 50;
                    if (ActivateUserSeed)
                    {
                        ProgressText = "Seed users...";
                        await SeedUsers();
                    }

                    Progress = 60;
                    if (ActivateUserImageSeed)
                    {
                        ProgressText = "Seed user images...";
                        await SeedUserImages();
                    }

                    Progress = 70;
                    if (ActivateWorkspaceSeed)
                    {
                        ProgressText = "Seed workspaces...";
                        await SeedWorkspaces();
                    }

                    Progress = 80;
                    if (ActivateUserDataSeed)
                    {
                        ProgressText = "Seed workspace data...";
                        await SeedWorkspaceData();
                    }

                    Progress = 90;
                    if (ActivateBookingSeed)
                    {
                        ProgressText = "Seed booking data...";
                        await SeedBookings();
                    }

                    Progress = 100;
                    ProgressText = "Done";
                }
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

            StepProgress = 0;
            var count = 0;
            foreach (var user in users)
            {
                await _userManager.Save(user);

                count++;
                StepProgress = count * 100 / users.Length;
            }

            StepProgress = 100;
        }

        private async Task SeedWorkspaces()
        {
            var json = File.ReadAllText("seeddata\\workspaces.json");
            var workspaces = JsonConvert.DeserializeObject<WorkspaceModel[]>(json);

            StepProgress = 0;
            var count = 0;
            foreach (var workspace in workspaces)
            {
                await _workspaceManager.Save(workspace);

                count++;
                StepProgress = count * 100 / workspaces.Length;
            }
            StepProgress = 100;
        }

        private async Task SeedUserImages()
        {
            var users = await _userManager.GetList();
            var user = users.FirstOrDefault();

            var filePaths = Directory.GetFiles("sampleimages");

            StepProgress = 0;
            var count = 0;
            foreach (var filePath in filePaths)
            {
                await _userImageManager.Save(user.Id, filePath);

                count++;
                StepProgress = count * 100 / filePaths.Length;
            }
            StepProgress = 100;
        }

        private async Task SeedWorkspaceData()
        {
            var workspaces = await _workspaceManager.GetList();
            var workspace = workspaces.FirstOrDefault();

            var days= 30;
            var hours = 24;
            var quaters = 4;

            StepProgress = 0;
            var count = 0;
            var maxCount = days*hours*quaters;

            for (int i = 1; i <= days; i++)
            {
                for (int j = 1; j < hours; j++)
                {
                    for (int k = 1; k < quaters; k++)
                    {

                        var model = new WorkspaceDataModel
                        {
                            WorkspaceId = workspace.Id,
                            Timestamp = new DateTime(2023,03,i, j, k*15, 0),
                            Temperature = Random.Next(15, 25),
                            Noise = Random.Next(60, 70),
                            Humidity= Random.Next(40, 60),
                            Co2 = Random.Next(400, 999),
                            Luminosity = Random.Next(100, 400)
                        };
                        await _workspaceDataManager.Save(model);

                        count++;
                        StepProgress = count * 100 / maxCount;
                    }
                }
            }

            StepProgress = 100;
        }

        private async Task SeedBookings()
        {
            StepProgress = 0;

            var users = await _userManager.GetList();
            var workspaces = await _workspaceManager.GetList();

            var workspace = workspaces.FirstOrDefault();
            var user = users.FirstOrDefault();

            var booking = new BookingModel
            {
                StartDateTime = DateTime.Now.AddMinutes(15),
                EndDateTime = DateTime.Now.AddHours(1),
                WorkspaceId = workspace.Id,
                WorkspaceNumber = workspace.WorkspaceNumber,
                RoomNumber = workspace.RoomNumber,
                UserId = user.Id,
                FirstName = user.FirstName ,
                LastName = user.LastName ,
                UserName =  user.UserName,
                Email = user.Email
            };

            await _bookingManager.Save(booking);

            StepProgress = 100;
        }

        private async Task DeleteUsers()
        {
            var users = await _userManager.GetList();

            StepProgress = 0;
            var count = 0;

            foreach (var user in users)
            {
                await _userManager.Delete(user.Id);

                count++;
                StepProgress = count * 100 / users.Length;
            }

            StepProgress = 100;
        }

        private async Task DeleteWorkspaces()
        {
            var workspaces = await _workspaceManager.GetList();

            StepProgress = 0;
            var count = 0;

            foreach (var workspace in workspaces)
            {
                await _workspaceManager.Delete(workspace.Id);

                count++;
                StepProgress = count * 100 / workspaces.Length;
            }

            StepProgress = 100;
        }

        private async Task DeleteUserImages()
        {
            var users = await _userManager.GetList();

            StepProgress = 0;
            var count = 0;

            foreach (var user in users)
            {
                await _userImageManager.Delete(user.Id);

                count++;
                StepProgress = count * 100 / users.Length;
            }

            StepProgress = 100;
        }

        private async Task DeleteWorkspaceData()
        {
            var workspaceData = await _workspaceDataManager.GetList();

            StepProgress = 0;
            var count = 0;

            foreach (var data in workspaceData)
            {
                await _workspaceDataManager.Delete(data.Id);

                count++;
                StepProgress = count * 100 / workspaceData.Length;
            }

            StepProgress = 100;
        }

        private async Task DeleteBookings()
        {
            var bookings = await _bookingManager.GetList();

            StepProgress = 0;
            var count = 0;

            foreach (var booking in bookings)
            {
                await _bookingManager.Delete(booking.Id);

                count++;
                StepProgress = count * 100 / bookings.Length;
            }

            StepProgress = 100;
        }
    }
}
