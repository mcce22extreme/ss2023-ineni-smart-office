using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class UserListViewModel : ListViewModelBase<UserModel>
    {
        private readonly IUserManager _userManager;

        public UserListViewModel(IUserManager userManager, IDialogService dialogService)
            : base(dialogService)
        {
            _userManager = userManager;
        }

        protected override async Task OnAdd()
        {
            await DialogService.ShowDialog(new UserDetailViewModel(_userManager, DialogService));
        }

        protected override async Task OnEdit()
        {
            await DialogService.ShowDialog(new UserDetailViewModel(SelectedItem, _userManager, DialogService));
        }

        protected override async Task OnDelete()
        {
            await _userManager.Delete(SelectedItem.Id);
        }

        protected override async Task<UserModel[]> OnReload()
        {
            return await _userManager.GetList();
        }
    }
}
