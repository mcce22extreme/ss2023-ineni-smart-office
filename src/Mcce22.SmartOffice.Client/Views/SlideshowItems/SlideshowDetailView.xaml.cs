using System.Windows;
using System.Windows.Controls;
using Mcce22.SmartOffice.Client.ViewModels;

namespace Mcce22.SmartOffice.Client.Views
{
    /// <summary>
    /// Interaction logic for SlideshotDetailViewModel.xaml
    /// </summary>
    public partial class SlideshowDetailView : UserControl
    {
        public SlideshowDetailView()
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
