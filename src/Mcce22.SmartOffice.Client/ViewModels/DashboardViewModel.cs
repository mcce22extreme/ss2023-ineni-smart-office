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

        //public ISeries[] Series1 { get; set; }
        //    = new ISeries[]
        //    {
        //        new LineSeries<double>
        //        {
        //            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
        //            Fill = null
        //        }
        //    };

        //public ISeries[] Series2 { get; set; }
        //    = new ISeries[]
        //    {
        //        new PieSeries<double> { Values = new double[] { 2 } },
        //        new PieSeries<double> { Values = new double[] { 4 } },
        //        new PieSeries<double> { Values = new double[] { 1 } },
        //        new PieSeries<double> { Values = new double[] { 4 } },
        //        new PieSeries<double> { Values = new double[] { 3 } }
        //    };

        //public ISeries[] Series3 { get; set; }
        //    = new ISeries[]
        //    {
        //        new LineSeries<int>
        //        {
        //            Values = new int[] { 4, 6, 5, 3, -3, -1, 2 }
        //        },
        //        new ColumnSeries<double>
        //        {
        //            Values = new double[] { 2, 5, 4, -2, 4, -3, 5 }
        //        }
        //    };
    }
}
