using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class ConfirmDeleteViewModel : DialogViewModelBase
    {
        public string Message { get; set; }

        public ConfirmDeleteViewModel(string title, string msg, IDialogService dialogService) : base(dialogService)
        {
            Title = title;
            Message = msg;
        }
    }
}
