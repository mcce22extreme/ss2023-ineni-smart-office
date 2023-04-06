using System;
using System.Windows;
using System.Windows.Controls;
using Mcce22.SmartOffice.Client.ViewModels;

namespace Mcce22.SmartOffice.Client.Views
{
    public partial class WorkspaceDetailView : UserControl
    {
        public WorkspaceDetailView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TbNumber.Focus();

            ((IDialogViewModel)DataContext).Load();
        }
    }
}
