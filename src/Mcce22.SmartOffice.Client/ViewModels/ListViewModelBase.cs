using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Mcce22.SmartOffice.Client.Services;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public interface IListViewModel
    {
        void Reload();
    }

    public abstract class ListViewModelBase<T> : ViewModelBase, IListViewModel
    {
        private ObservableCollection<T> _items = new ObservableCollection<T>();
        public ObservableCollection<T> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public RelayCommand AddCommand { get; }

        public RelayCommand EditCommand { get; }

        public RelayCommand DeleteCommand { get; }

        public RelayCommand ReloadCommand { get; }

        private T _selectedItem;
        public T SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        protected IDialogService DialogService { get; }

        public ListViewModelBase(IDialogService dialogService)
        {
            DialogService = dialogService;

            AddCommand = new RelayCommand(Add, CanAdd);
            EditCommand = new RelayCommand(Edit, CanEdit);
            ReloadCommand = new RelayCommand(Reload, CanReload);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }


        protected override void UpdateCommandStates()
        {
            AddCommand.NotifyCanExecuteChanged();
            EditCommand.NotifyCanExecuteChanged();
            ReloadCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }

        protected bool CanAdd()
        {
            return !IsBusy;
        }

        protected async void Add()
        {
            if (CanAdd())
            {
                await OnAdd();

                Reload();
            }
        }

        protected virtual Task OnAdd()
        {
            return Task.CompletedTask;
        }

        protected virtual bool CanEdit()
        {
            return !IsBusy && SelectedItem != null;
        }

        protected async void Edit()
        {
            if (CanEdit())
            {
                await OnEdit();

                Reload();
            }
        }

        protected virtual Task OnEdit()
        {
            return Task.CompletedTask;
        }

        protected bool CanDelete()
        {
            return !IsBusy && SelectedItem != null;
        }

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

        protected virtual Task OnDelete()
        {
            return Task.CompletedTask;
        }

        protected bool CanReload()
        {
            return !IsBusy;
        }

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

        protected virtual Task<T[]> OnReload()
        {
            return Task.FromResult(Array.Empty<T>());
        }
    }
}
