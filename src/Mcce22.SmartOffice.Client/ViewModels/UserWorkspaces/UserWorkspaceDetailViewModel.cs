using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class UserWorkspaceDetailViewModel : DialogViewModelBase
    {
        private readonly IUserWorkspaceManager _userWorkspaceManager;
        private readonly IWorkspaceManager _workspaceManager;
        private readonly IUserManager _userManager;

        private readonly int _workspaceId;
        private readonly int _userId;

        private long _deskHeight;
        public long DeskHeight
        {
            get { return _deskHeight; }
            set { SetProperty(ref _deskHeight, value); }
        }

        private string _slideshowResourceKey;
        public string SlideshowResourceKey
        {
            get { return _slideshowResourceKey; }
            set { SetProperty(ref _slideshowResourceKey, value); }
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

        public int UserWorkspaceId { get; }

        public UserWorkspaceDetailViewModel(
            IUserWorkspaceManager userWorkspaceManager,
            IWorkspaceManager workspaceManager,
            IUserManager userManager,
            IDialogService dialogService)
            : base(dialogService)
        {
            Title = "Create workspace configuration";

            _userWorkspaceManager = userWorkspaceManager;
            _workspaceManager = workspaceManager;
            _userManager = userManager;
        }

        public UserWorkspaceDetailViewModel(
            UserWorkspaceModel model,
            IUserWorkspaceManager userWorkspaceManager,
            IWorkspaceManager workspaceManager,
            IUserManager userManager,
            IDialogService dialogService)
            : this(userWorkspaceManager, workspaceManager, userManager, dialogService)
        {
            Title = "Edit workspace configuration";

            UserWorkspaceId = model.Id;
            DeskHeight = model.DeskHeight;

            _workspaceId = model.WorkspaceId;
            _userId = model.UserId;
        }

        public override async void Load()
        {
            try
            {
                IsBusy = true;

                var workspaces = await _workspaceManager.GetList();
                var users = await _userManager.GetList();

                Workspaces = new ObservableCollection<WorkspaceModel>(workspaces);
                Users = new ObservableCollection<UserModel>(users);

                SelectedWorkspace = _workspaceId > 0 ? workspaces.FirstOrDefault(x => x.Id == _workspaceId) : null;
                SelectedUser = _userId > 0 ? users.FirstOrDefault(x => x.Id == _userId) : null;
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task OnSave()
        {
            await _userWorkspaceManager.Save(new UserWorkspaceModel
            {
                Id = UserWorkspaceId,
                DeskHeight = DeskHeight,
                SlideshowResourceKey = SlideshowResourceKey,
                WorkspaceId = SelectedWorkspace.Id,
                UserId = SelectedUser.Id
            });
        }
    }
}
