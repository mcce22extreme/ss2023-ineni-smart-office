using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public partial class UserDetailViewModel : DialogViewModelBase
    {
        private readonly IUserManager _userManager;

        public string UserId { get; }

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _email;
        
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
