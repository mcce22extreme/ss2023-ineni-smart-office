using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class BookingDetailViewModel : DialogViewModelBase
    {
        private readonly IBookingManager _bookingManager;
        private readonly IUserManager _userManager;
        private readonly IWorkspaceManager _workspaceManager;

        private List<WorkspaceModel> _allWorkspaces = new List<WorkspaceModel>();
        private List<UserModel> _allUsers = new List<UserModel>();

        public int WorkspaceId { get; set; }

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

        private ObservableCollection<WorkspaceModel> _workspaces = new ObservableCollection<WorkspaceModel>();
        public ObservableCollection<WorkspaceModel> Workspaces
        {
            get { return _workspaces; }
            set { SetProperty(ref _workspaces, value); }
        }

        private ObservableCollection<UserModel> _users = new ObservableCollection<UserModel>();
        public ObservableCollection<UserModel> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

        private WorkspaceModel _selectedWorkspace;
        public WorkspaceModel SelectedWorkspace
        {
            get { return _selectedWorkspace; }
            set { SetProperty(ref _selectedWorkspace, value); }
        }

        private UserModel _selectedUser;
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        public BookingDetailViewModel(
            IBookingManager bookingManager,
            IUserManager userManager,
            IWorkspaceManager workspaceManager,
            IDialogService dialogService)
            : base(dialogService)
        {
            Title = "Create booking";

            _bookingManager = bookingManager;
            _userManager = userManager;
            _workspaceManager = workspaceManager;

            StartDateTime = DateTime.Now.AddMinutes(5);
            EndDateTime = DateTime.Now.AddMinutes(20);
        }

        public override async void Load()
        {
            try
            {
                IsBusy = true;

                _allWorkspaces.AddRange(await _workspaceManager.GetList());
                _allUsers.AddRange(await _userManager.GetList());

                Workspaces = new ObservableCollection<WorkspaceModel>(_allWorkspaces);
                Users = new ObservableCollection<UserModel>(_allUsers);

                SelectedWorkspace = WorkspaceId > 0 ? Workspaces.FirstOrDefault(x => x.Id == WorkspaceId) : null;
                SelectedUser = null;
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task OnSave()
        {
            await _bookingManager.Save(new BookingModel
            {
                StartDateTime = StartDateTime,
                EndDateTime = EndDateTime,
                WorkspaceId = SelectedWorkspace.Id,
                UserId = SelectedUser.Id
            });

        }
    }
}
