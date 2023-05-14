using System.Windows;
using System.Windows.Controls;
using Mcce22.SmartOffice.Client.ViewModels;

namespace Mcce22.SmartOffice.Client.Views
{
    /// <summary>
    /// Interaction logic for WorkspaceDataListView.xaml
    /// </summary>
    public partial class WorkspaceDataListView : UserControl
    {
        public WorkspaceDataListView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ((IListViewModel)DataContext).Reload();
        }
    }
}
