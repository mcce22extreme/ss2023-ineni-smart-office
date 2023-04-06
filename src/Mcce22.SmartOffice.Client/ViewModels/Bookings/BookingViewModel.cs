using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IBookingManager _bookingManager;
        private readonly IUserManager _userManager;
        private readonly IDialogService _dialogService;

        public ISeries[] Series1 { get; set; }
            = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = "Temperature",
                    Values = new double[] { 20, 21, 19, 16, 16, 18, 22, 21, 24, 18 },
                    Fill = null,

                }
            };

        public ISeries[] Series2 { get; set; }
            = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = "Loudness",
                    Values = new double[] { 45, 43, 40, 20, 20, 45, 41, 42, 55, 40 },
                    
                },
                new ColumnSeries<double>
                {
                    Name = "Bookings",
                    Values = new double[] { 3, 5, 4, 0, 0, 2, 5, 5, 7, 3 }
                }
            };

        private ObservableCollection<WorkspaceModel> _workspaces = new ObservableCollection<WorkspaceModel>();
        public ObservableCollection<WorkspaceModel> Workspaces
        {
            get { return _workspaces; }
            set { SetProperty(ref _workspaces, value); }
        }

        private ObservableCollection<BookingModel> _bookings = new ObservableCollection<BookingModel>();
        public ObservableCollection<BookingModel> Bookings
        {
            get { return _bookings; }
            set { SetProperty(ref _bookings, value); }
        }

        private WorkspaceModel _selectedWorkspace;
        public WorkspaceModel SelectedWorkspace
        {
            get { return _selectedWorkspace; }
            set { SetProperty(ref _selectedWorkspace, value); }
        }

        private BookingModel _selectedBooking;
        public BookingModel SelectedBooking
        {
            get { return _selectedBooking; }
            set
            {
                if (SetProperty(ref _selectedBooking, value) && value != null)
                {
                    SelectedWorkspace = Workspaces.FirstOrDefault(x => x.Id == _selectedBooking.WorkspaceId);
                    UpdateCommandStates();
                }
            }
        }

        public RelayCommand AddBookingCommand { get; }

        public RelayCommand DeleteBookingCommand { get; set; }

        public RelayCommand ReloadCommand { get; }

        public BookingViewModel(IWorkspaceManager workspaceManager, IBookingManager bookingManager, IUserManager userManager, IDialogService dialogService)
        {
            _workspaceManager = workspaceManager;
            _bookingManager = bookingManager;
            _userManager = userManager;
            _dialogService = dialogService;

            AddBookingCommand = new RelayCommand(AddBooking, CanAddBooking);
            DeleteBookingCommand = new RelayCommand(DeleteBooking, CanDeleteBooking);
            ReloadCommand = new RelayCommand(Reload, CanReload);
        }

        private bool CanDeleteBooking()
        {
            return !IsBusy && SelectedBooking != null;
        }

        private async void DeleteBooking()
        {
            if (CanDeleteBooking())
            {
                try
                {
                    IsBusy = true;

                    var confirmDelete = new ConfirmDeleteViewModel("Delete booking", "Do you really want to delete the selected entry?", _dialogService);

                    await _dialogService.ShowDialog(confirmDelete);

                    if (confirmDelete.Confirmed)
                    {
                        await _bookingManager.Delete(SelectedBooking.Id);
                        await LoadBookings();
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        protected override void UpdateCommandStates()
        {
            base.UpdateCommandStates();

            AddBookingCommand.NotifyCanExecuteChanged();
            DeleteBookingCommand.NotifyCanExecuteChanged();
            ReloadCommand.NotifyCanExecuteChanged();
        }

        private bool CanReload()
        {
            return !IsBusy;
        }

        private async void Reload()
        {
            if (CanReload())
            {
                try
                {
                    IsBusy = true;
                    await Load();
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task Load()
        {
            await LoadWorkspaces();
            await LoadBookings();
        }

        private async Task LoadWorkspaces()
        {
            var workspaces = await _workspaceManager.GetList();
            Workspaces = new ObservableCollection<WorkspaceModel>(workspaces);
        }

        private async Task LoadBookings()
        {
            var bookings = await _bookingManager.GetList();


            Bookings = new ObservableCollection<BookingModel>(bookings);
        }

        private bool CanAddBooking()
        {
            return !IsBusy;
        }

        private async void AddBooking()
        {
            if (CanAddBooking())
            {
                await _dialogService.ShowDialog(new BookingDetailViewModel(
                    _bookingManager,
                    _userManager,
                    _workspaceManager,
                    _dialogService)
                {
                    WorkspaceId = SelectedWorkspace?.Id ?? 0
                });

                await LoadBookings();
            }
        }
    }
}
