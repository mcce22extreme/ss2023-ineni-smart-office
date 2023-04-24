using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class CreateBookingViewModel : ViewModelBase
    {
        private List<BookingModel> _allBookings = new List<BookingModel>();

        private DateTime _startDateTime;
        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { SetProperty(ref _startDateTime, value); }
        }

        private DateTime _endDateTime;
        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set { SetProperty(ref _endDateTime, value); }
        }

        private List<UserModel> _userModels;

        public List<UserModel> Users
        {
            get { return _userModels; }
            set { SetProperty(ref _userModels, value); }
        }

        private UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        private List<WorkspaceModel> _workspaces;
        public List<WorkspaceModel> Workspaces
        {
            get { return _workspaces; }
            set { SetProperty(ref _workspaces, value); }
        }

        private List<BookingModel> _bookings;
        public List<BookingModel> Bookings
        {
            get { return _bookings; }
            set { SetProperty(ref _bookings, value); }
        }

        private bool _workspacesAvailable;
        public bool WorkspacesAvailable
        {
            get { return _workspacesAvailable; }
            set { SetProperty(ref _workspacesAvailable, value); }
        }

        private WorkspaceModel _selectedWorkspace;
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IBookingManager _bookingManager;
        private readonly IUserManager _userManager;

        public WorkspaceModel SelectedWorkspace
        {
            get { return _selectedWorkspace; }
            set
            {
                if (SetProperty(ref _selectedWorkspace, value))
                {
                    UpdateBookings();
                }
            }
        }

        public event EventHandler WorkspaceAvailabilityUpdated;

        public RelayCommand UpdateAvailabilityCommand { get; }

        public RelayCommand CreateBookingCommand { get; }

        public CreateBookingViewModel(IWorkspaceManager workspaceManager, IBookingManager bookingManager, IUserManager userManager)
        {
            _workspaceManager = workspaceManager;
            _bookingManager = bookingManager;
            _userManager = userManager;

            UpdateAvailabilityCommand = new RelayCommand(UpdateAvailability, CanUpdateAvailability);
            CreateBookingCommand = new RelayCommand(CreateBooking, CanCreateBooking);

            var dateTimeNow = DateTime.Now;
            StartDateTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 6, 0, 0);
            EndDateTime = new DateTime(dateTimeNow.Year, dateTimeNow.Month, dateTimeNow.Day, 18, 0, 0);
        }

        protected override void UpdateCommandStates()
        {
            base.UpdateCommandStates();

            UpdateAvailabilityCommand.NotifyCanExecuteChanged();
            CreateBookingCommand.NotifyCanExecuteChanged();
        }

        public async Task Load()
        {
            try
            {
                IsBusy = true;

                var workspaces = await _workspaceManager.GetList();
                var users = await _userManager.GetList();

                Workspaces = new List<WorkspaceModel>(workspaces);
                Users = new List<UserModel>(users);

                SelectedUser = Users.FirstOrDefault();

                UpdateAvailability();
            }
            finally
            {
                IsBusy = false;
            }            
        }

        private bool CanUpdateAvailability()
        {
            return !IsBusy;
        }

        private async void UpdateAvailability()
        {
            try
            {
                IsBusy = true;

                SelectedWorkspace = null;

                _allBookings = new List<BookingModel>(await _bookingManager.GetList());

                foreach (var workspace in Workspaces)
                {
                    workspace.IsAvailable = !_allBookings.Any(x =>
                        x.WorkspaceId == workspace.Id &&
                        x.StartDateTime.Date == StartDateTime.Date &&
                        ((x.StartDateTime.TimeOfDay == StartDateTime.TimeOfDay && x.EndDateTime.TimeOfDay == EndDateTime.TimeOfDay) ||
                        (x.StartDateTime.TimeOfDay <= StartDateTime.TimeOfDay && x.EndDateTime.TimeOfDay > StartDateTime.TimeOfDay) ||
                        (x.StartDateTime.TimeOfDay >= StartDateTime.TimeOfDay && x.EndDateTime.TimeOfDay <= EndDateTime.TimeOfDay)));
                }

                WorkspaceAvailabilityUpdated?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanCreateBooking()
        {
            return !IsBusy && SelectedUser != null && SelectedWorkspace?.IsAvailable == true;
        }

        private async void CreateBooking()
        {
            try
            {
                IsBusy = true;

                await _bookingManager.Save(new BookingModel
                {
                    StartDateTime = StartDateTime,
                    EndDateTime = EndDateTime,
                    WorkspaceId = SelectedWorkspace.Id,
                    UserId = SelectedUser.Id
                });
            }
            finally
            {
                IsBusy = false;
            }

            UpdateAvailability();
        }

        private void UpdateBookings()
        {
            try
            {
                IsBusy = true;

                if(SelectedWorkspace == null)
                {
                    Bookings = new List<BookingModel>();
                }
                else
                {
                    Bookings = _allBookings.Where(x => x.WorkspaceId == SelectedWorkspace.Id).ToList();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
