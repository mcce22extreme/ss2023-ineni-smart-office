using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _loginVisible = true;

        [ObservableProperty]
        private string _userName = "demo";

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _isAdmin;
        
        public ICommand LoginAsAdminCommand { get; }

        public ICommand LoginAsUserCommand { get; }

        public event EventHandler LoginChanged;

        public LoginViewModel()
        {
            LoginAsAdminCommand = new RelayCommand(() => Login(true));
            LoginAsUserCommand = new RelayCommand(() => Login(false));
        }

        public void Login(bool asAdmin)
        {
            IsAdmin = asAdmin;
            LoginChanged?.Invoke(this, EventArgs.Empty);
            LoginVisible = false;           
        }
    }
}
