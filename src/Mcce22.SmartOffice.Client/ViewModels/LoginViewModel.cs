using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private bool _loginVisible = true;
        public bool LoginVisible
        {
            get { return _loginVisible; }
            set { SetProperty(ref _loginVisible, value); }
        }

        private string _userName = "demo";
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { SetProperty(ref _isAdmin, value); }
        }

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
