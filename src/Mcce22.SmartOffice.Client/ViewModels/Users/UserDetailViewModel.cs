using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class UserDetailViewModel : DialogViewModelBase
    {
        private readonly IUserManager _userManager;

        public string UserId { get; }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public UserDetailViewModel(IUserManager userManager, IDialogService dialogService)
            : base(dialogService)
        {
            Title = "Create user";
            _userManager = userManager;
        }

        public UserDetailViewModel(UserModel user, IUserManager userManager, IDialogService dialogService)
            : base(dialogService)
        {
            Title = "Edit user";
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Email = user.Email;

            _userManager = userManager;
        }

        protected override async Task OnSave()
        {
            await _userManager.Save(new UserModel
            {
                Id = UserId,
                FirstName = FirstName,
                LastName = LastName,
                UserName = UserName,
                Email = Email
            });
        }
    }
}
