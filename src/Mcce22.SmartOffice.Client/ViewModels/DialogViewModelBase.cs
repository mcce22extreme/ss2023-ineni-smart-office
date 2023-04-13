using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public interface IDialogViewModel
    {
        string Title { get; }

        void Load();
    }

    public abstract class DialogViewModelBase : ViewModelBase, IDialogViewModel
    {
        protected IDialogService DialogService { get; }

        public RelayCommand SaveCommand { get; }

        public RelayCommand CancelCommand { get; }

        public string Title { get; protected set; }

        public bool Confirmed { get; protected set; }

        public DialogViewModelBase(IDialogService dialogService)
        {
            DialogService = dialogService;
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
        }

        public virtual void Load()
        {
        }

        protected bool CanSave()
        {
            return !IsBusy;
        }

        protected async void Save()
        {
            if (CanSave())
            {
                try
                {
                    IsBusy = true;

                    await OnSave();

                    Confirmed = true;
                    DialogService.CloseDialog(this);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    await DialogService.ShowDialog(new ErrorViewModel(ex, DialogService));
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        protected virtual Task OnSave()
        {
            return Task.CompletedTask;
        }

        protected bool CanCancel()
        {
            return !IsBusy;
        }

        protected void Cancel()
        {
            if (CanCancel())
            {
                DialogService.CloseDialog(this);
            }
        }
    }
}
