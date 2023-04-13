using System.Windows;
using System.Windows.Controls;
using Mcce22.SmartOffice.Client.ViewModels;

namespace Mcce22.SmartOffice.Client.Views
{
    /// <summary>
    /// Interaction logic for UserImageDetailView.xaml
    /// </summary>
    public partial class UserImageDetailView : UserControl
    {
        public UserImageDetailView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CBUsers.Focus();

            ((IDialogViewModel)DataContext).Load();
        }
    }
}
