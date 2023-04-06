using System.Threading.Tasks;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class SlideshowItemListViewModel : ListViewModelBase<SlideshowItemModel>
    {
        private readonly ISlideshowItemManager _slideshowItemManager;
        private readonly IUserManager _userManager;

        public SlideshowItemListViewModel(ISlideshowItemManager slideshowItemManager, IUserManager userManager, IDialogService dialogService)
            : base(dialogService)
        {
            _slideshowItemManager = slideshowItemManager;
            _userManager = userManager;
        }

        protected override async Task OnAdd()
        {
            await DialogService.ShowDialog(new SlideshowDetailViewModel(_slideshowItemManager, _userManager, DialogService));
        }

        protected override async Task OnDelete()
        {
            await _slideshowItemManager.Delete(SelectedItem.Id);
        }

        protected override Task<SlideshowItemModel[]> OnReload()
        {
            return _slideshowItemManager.GetList();
        }
    }
}
