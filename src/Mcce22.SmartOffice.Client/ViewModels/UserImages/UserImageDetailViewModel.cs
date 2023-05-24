using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;
using Microsoft.Win32;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public partial class UserImageDetailViewModel : DialogViewModelBase
    {
        private readonly IUserImageManager _userImageManager;
        private readonly IUserManager _userManager;

        [ObservableProperty]
        private ObservableCollection<UserModel> _users = new ObservableCollection<UserModel>();

        [ObservableProperty]
        private UserModel _selectedUser;

        [ObservableProperty]
        private string _filePath;
        
        public UserImageDetailViewModel(IUserImageManager userImageManager, IUserManager userManager, IDialogService dialogService)
            : base(dialogService)
        {
            Title = "Add user image";

            _userImageManager = userImageManager;
            _userManager = userManager;        
        }

        [RelayCommand]
        private void SelectFile()
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;
            }
        }

        public override async void Load()
        {
            try
            {
                IsBusy = true;
                var users = await _userManager.GetList();

                Users = new ObservableCollection<UserModel>(users);

                SelectedUser = null;
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task OnSave()
        {
            await _userImageManager.Save(SelectedUser.Id, FilePath);
        }
    }
}
