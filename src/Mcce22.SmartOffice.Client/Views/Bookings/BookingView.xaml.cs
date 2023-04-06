using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Mcce22.SmartOffice.Client.Models;
using Mcce22.SmartOffice.Client.ViewModels;

namespace Mcce22.SmartOffice.Client.Views
{
    public partial class BookingView : UserControl
    {
        private static Brush DefaultBrush = Brushes.Transparent;
        private static Brush SelectedBrush = Brushes.Orange;

        private Rectangle _selectedRect;

        public BookingView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dataContext = e.OldValue as BookingViewModel;
            if (dataContext != null)
            {
                dataContext.PropertyChanged -= OnDataContextPropertyChanged;
            }

            dataContext = e.NewValue as BookingViewModel;
            if (dataContext != null)
            {
                dataContext.PropertyChanged += OnDataContextPropertyChanged;
            }
        }

        private void OnDataContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var dataContext = DataContext as BookingViewModel;

            if (e.PropertyName == nameof(BookingViewModel.SelectedWorkspace))
            {
                var rect = WorkspaceCanvas.Children.OfType<Rectangle>().FirstOrDefault(x => x.Tag == dataContext.SelectedWorkspace);

                if (_selectedRect != null)
                {
                    _selectedRect.Fill = DefaultBrush;
                }

                if (rect != null)
                {
                    _selectedRect = rect;
                    _selectedRect.Fill = Brushes.Orange;
                }
            }
            else if (e.PropertyName == nameof(BookingViewModel.Workspaces))
            {
                WorkspaceCanvas.ResetZoom();

                var rects = WorkspaceCanvas.Children.OfType<Rectangle>().ToList();

                foreach (var rect in rects)
                {
                    WorkspaceCanvas.Children.Remove(rect);
                }

                foreach (var workspace in dataContext.Workspaces)
                {
                    var rect = new Rectangle
                    {
                        Fill = DefaultBrush,
                        Opacity= 0.5,
                        Width = workspace.Width,
                        Height = workspace.Height,
                        Tag = workspace
                    };

                    WorkspaceCanvas.Children.Add(rect);

                    Canvas.SetTop(rect, workspace.Top);
                    Canvas.SetLeft(rect, workspace.Left);
                }                
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ((BookingViewModel)DataContext).Load();
        }

        private void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var dataContext = DataContext as BookingViewModel;

                if (_selectedRect != null)
                {
                    _selectedRect.Fill = DefaultBrush;
                }

                if (_selectedRect == e.Source)
                {
                    _selectedRect = null;
                    dataContext.SelectedWorkspace = null;

                }
                else
                {
                    _selectedRect = e.Source as Rectangle;
                    if (_selectedRect != null)
                    {
                        _selectedRect.Fill = SelectedBrush;
                        dataContext.SelectedWorkspace = _selectedRect.Tag as WorkspaceModel;
                    }
                }
            }
        }
    }
}
