using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class UserImageListViewModel : ListViewModelBase<UserImageModel>
    {
        private readonly IUserImageManager _userImageManager;
        private readonly IUserManager _userManager;

        public UserImageListViewModel(IUserImageManager userImageManager, IUserManager userManager, IDialogService dialogService)
            : base(dialogService)
        {
            _userImageManager = userImageManager;
            _userManager = userManager;
        }

        protected override async Task OnAdd()
        {
            await DialogService.ShowDialog(new UserImageDetailViewModel(_userImageManager, _userManager, DialogService));
        }

        protected override async Task OnDelete()
        {
            await _userImageManager.Delete(SelectedItem.Id);
        }

        protected override Task<UserImageModel[]> OnReload()
        {
            return _userImageManager.GetList();
        }
    }
}
