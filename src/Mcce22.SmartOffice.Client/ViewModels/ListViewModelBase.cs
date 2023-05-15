using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public interface IListViewModel
    {
        void Reload();
    }

    public abstract partial class ListViewModelBase<T> : ViewModelBase, IListViewModel
    {
        [ObservableProperty]
        private ObservableCollection<T> _items = new ObservableCollection<T>();

        [ObservableProperty]
        private T _selectedItem;

        protected IDialogService DialogService { get; }

        public ListViewModelBase(IDialogService dialogService)
        {
            DialogService = dialogService;
        }

        protected override void UpdateCommandStates()
        {
            AddCommand.NotifyCanExecuteChanged();
            EditCommand.NotifyCanExecuteChanged();
            ReloadCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanAdd))]
        protected async void Add()
        {
            if (CanAdd())
            {
                await OnAdd();

                Reload();
            }
        }

        protected bool CanAdd()
        {
            return !IsBusy;
        }

        protected virtual Task OnAdd()
        {
            return Task.CompletedTask;
        }

        [RelayCommand(CanExecute = nameof(CanEdit))]
        protected async void Edit()
        {
            if (CanEdit())
            {
                await OnEdit();

                Reload();
            }
        }

        protected virtual bool CanEdit()
        {
            return !IsBusy && SelectedItem != null;
        }

        protected virtual Task OnEdit()
        {
            return Task.CompletedTask;
        }


        [RelayCommand(CanExecute = nameof(CanDelete))]
        protected async void Delete()
        {
            if (CanDelete())
            {
                try
                {
                    IsBusy = true;

                    var confirmDelete = new ConfirmDeleteViewModel("Delete selected entry?", "Do you really want to delete the selected entry?", DialogService);

                    await DialogService.ShowDialog(confirmDelete);

                    if (confirmDelete.Confirmed)
                    {
                        await OnDelete();
                    }
                }
                finally
                {
                    IsBusy = false;
                }

                Reload();
            }
        }

        protected bool CanDelete()
        {
            return !IsBusy && SelectedItem != null;
        }

        protected virtual Task OnDelete()
        {
            return Task.CompletedTask;
        }

        [RelayCommand(CanExecute = nameof(CanReload))]
        public async void Reload()
        {
            if (CanReload())
            {
                try
                {
                    IsBusy = true;

                    var items = await OnReload();
                    Items = new ObservableCollection<T>(items);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        protected bool CanReload()
        {
            return !IsBusy;
        }

        protected virtual Task<T[]> OnReload()
        {
            return Task.FromResult(Array.Empty<T>());
        }
    }
}
