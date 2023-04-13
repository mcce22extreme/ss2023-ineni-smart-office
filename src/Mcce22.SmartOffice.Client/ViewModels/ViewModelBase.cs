using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Mcce22.SmartOffice.Client.ViewModels
{
    public class ViewModelBase : ObservableObject
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetProperty(ref _isBusy, value))
                {
                    UpdateCommandStates();
                }
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            UpdateCommandStates();
        }

        protected virtual void UpdateCommandStates()
        {
        }
    }
}
