using System;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class ErrorViewModel : DialogViewModelBase
    {
        public string Message { get; }

        public ErrorViewModel(string message, IDialogService dialogService)
            : base(dialogService)
        {
            Title = "Error occurred...";
            Message = message;
        }

        public ErrorViewModel(Exception exception, IDialogService dialogService)
            : this(exception?.Message, dialogService)
        {
        }
    }
}
