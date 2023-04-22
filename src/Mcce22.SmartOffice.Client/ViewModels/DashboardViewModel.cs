using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Enums;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { SetProperty(ref _isAdmin, value); }
        }

        public ICommand NavigateCommand { get; }

        public DashboardViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigateCommand = new RelayCommand<NavigationType>(Navigate);
        }

        private void Navigate(NavigationType type)
        {
            _navigationService.Navigate(type);
        }
    }
}
