using System.Windows;
using System.Windows.Controls;
using Mcce22.SmartOffice.Client.ViewModels;

namespace Mcce22.SmartOffice.Client.Views
{
    public partial class WorkspaceConfigurationDetailView : UserControl
    {
        public WorkspaceConfigurationDetailView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NUDDeskHeight.Focus();

            ((IDialogViewModel)DataContext).Load();
        }
    }
}
