using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class BookingListViewModel : ListViewModelBase<BookingModel>
    {
        private readonly IBookingManager _bookingManager;
        private readonly IProcessBookingManager _processBookingManager;

        public RelayCommand ProcessBookingsCommand { get; }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { SetProperty(ref _isAdmin, value); }
        }

        public BookingListViewModel(
            IBookingManager bookingManager,
            IProcessBookingManager processBookingManager,
            IDialogService dialogService)
            : base(dialogService)
        {
            _bookingManager = bookingManager;
            _processBookingManager = processBookingManager;
            ProcessBookingsCommand = new RelayCommand(ProcessBookings, CanProcessBookings);
        }

        private bool CanProcessBookings()
        {
            return !IsBusy;
        }

        private async void ProcessBookings()
        {
            if (CanProcessBookings())
            {
                try
                {
                    IsBusy = true;
                    await _processBookingManager.ProcessBookings();
                }
                finally
                {
                    IsBusy = false;
                }

                Reload();
            }
        }

        protected override bool CanEdit()
        {
            return !IsBusy && SelectedItem != null && !SelectedItem.InvitationSent;
        }

        protected override async Task OnDelete()
        {
            await _bookingManager.Delete(SelectedItem.Id);
        }

        protected override async Task<BookingModel[]> OnReload()
        {
            return await _bookingManager.GetList();
        }
    }
}
