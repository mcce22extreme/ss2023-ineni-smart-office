using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;
using Microsoft.Win32;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    internal class UserImageDetailViewModel : DialogViewModelBase
    {
        private readonly IUserImageManager _userImageManager;
        private readonly IUserManager _userManager;

        private ObservableCollection<UserModel> _users = new ObservableCollection<UserModel>();
        public ObservableCollection<UserModel> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        private UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        public RelayCommand SelectFileCommand { get; }

        public UserImageDetailViewModel(IUserImageManager userImageManager, IUserManager userManager, IDialogService dialogService)
            : base(dialogService)
        {
            Title = "Add user image";
            _userImageManager = userImageManager;
            _userManager = userManager;

            SelectFileCommand = new RelayCommand(SelectFile);
        }

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
