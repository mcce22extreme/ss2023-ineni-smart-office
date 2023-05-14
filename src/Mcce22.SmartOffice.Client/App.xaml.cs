using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Services;
using Mcce22.SmartOffice.Client.ViewModels;

namespace Mcce22.SmartOffice.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly WindsorContainer _container = new WindsorContainer();

        private void OnStartUp(object sender, StartupEventArgs e)
        {
            _container.Register(Component.For<IDialogService>().ImplementedBy<DialogService>().LifestyleSingleton());
            _container.Register(Component.For<INavigationService>().ImplementedBy<NavigationService>().LifestyleSingleton());

            _container.Register(Component.For<MainWindow>());

            _container.Register(Component.For<MainViewModel>().LifestyleSingleton());
            _container.Register(Component.For<LoginViewModel>().LifestyleSingleton());
            _container.Register(Component.For<DashboardViewModel>().LifestyleSingleton());
            _container.Register(Component.For<UserListViewModel>().LifestyleSingleton());
            _container.Register(Component.For<WorkspaceListViewModel>().LifestyleSingleton());
            _container.Register(Component.For<BookingListViewModel>().LifestyleSingleton());
            _container.Register(Component.For<UserWorkspaceListViewModel>().LifestyleSingleton());
            _container.Register(Component.For<UserImageListViewModel>().LifestyleSingleton());
            _container.Register(Component.For<SeedDataViewModel>().LifestyleSingleton());
            _container.Register(Component.For<CreateBookingViewModel>().LifestyleSingleton());
            _container.Register(Component.For<WorkspaceDataListViewModel>().LifestyleSingleton());

            _container.Register(Component.For<IUserManager>()
                .ImplementedBy<UserManager>()
                .LifestyleSingleton()
                .DependsOn(Dependency.OnValue("baseUrl", AppSettings.Current.BaseAddressUsers)));          

            _container.Register(Component.For<IWorkspaceManager>()
                .ImplementedBy<WorkspaceManager>()
                .LifestyleSingleton()
                .DependsOn(Dependency.OnValue("baseUrl", AppSettings.Current.BaseAddressWorkspaces)));

            _container.Register(Component.For<IBookingManager>()
                .ImplementedBy<BookingManager>()
                .LifestyleSingleton()
                .DependsOn(Dependency.OnValue("baseUrl", AppSettings.Current.BaseAddressBookings)));

            _container.Register(Component.For<IUserWorkspaceManager>()
                .ImplementedBy<UserWorkspaceManager>()
                .LifestyleSingleton()
                .DependsOn(Dependency.OnValue("baseUrl", AppSettings.Current.BaseAddressWorkspaces)));

            _container.Register(Component.For<IUserImageManager>()
                .ImplementedBy<UserImageManager>()
                .LifestyleSingleton()
                .DependsOn(Dependency.OnValue("baseUrl", AppSettings.Current.BaseAddressUsers)));

            _container.Register(Component.For<IWorkspaceDataManager>()
                .ImplementedBy<WorkspaceDataManager>()
                .LifestyleSingleton()
                .DependsOn(Dependency.OnValue("baseUrl", AppSettings.Current.BaseAddressWorkspaces)));

            _container.Register(Component.For<IProcessBookingManager>()
                .ImplementedBy<ProcessBookingManager>()
                .LifestyleSingleton()
                .DependsOn(Dependency.OnValue("baseUrl", AppSettings.Current.BaseAddressNotifications)));

            MainWindow = _container.Resolve<MainWindow>();
            MainWindow.Show();
        }
    }
}
