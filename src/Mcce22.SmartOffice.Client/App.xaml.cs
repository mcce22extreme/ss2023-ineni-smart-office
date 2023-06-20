using System;
using System.Windows;
using Mcce22.SmartOffice.Client.Managers;
using Mcce22.SmartOffice.Client.Services;
using Mcce22.SmartOffice.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Mcce22.SmartOffice.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        //private readonly WindsorContainer _container = new();

        private void OnStartUp(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<MainWindow>();

            services.AddSingleton<MainViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<UserListViewModel>();
            services.AddSingleton<WorkspaceListViewModel>();
            services.AddSingleton<BookingListViewModel>();
            services.AddSingleton<WorkspaceConfigurationListViewModel>();
            services.AddSingleton<UserImageListViewModel>();
            services.AddSingleton<SeedDataViewModel>();
            services.AddSingleton<CreateBookingViewModel>();
            services.AddSingleton<WorkspaceDataListViewModel>();

            services.AddSingleton<IUserManager, UserManager>(s => new UserManager(AppSettings.Current.BaseAddressUsers));
            services.AddSingleton<IWorkspaceManager, WorkspaceManager>(s => new WorkspaceManager(AppSettings.Current.BaseAddressWorkspaces));
            services.AddSingleton<IBookingManager, BookingManager>(s => new BookingManager(AppSettings.Current.BaseAddressBookings));
            services.AddSingleton<IWorkspaceConfigurationManager, WorkspaceConfigurationManager>(s => new WorkspaceConfigurationManager(AppSettings.Current.BaseAddressWorkspaces));
            services.AddSingleton<IUserImageManager, UserImageManager>(s => new UserImageManager(AppSettings.Current.BaseAddressUsers));
            services.AddSingleton<IWorkspaceDataManager, WorkspaceDataManager>(s => new WorkspaceDataManager(AppSettings.Current.BaseAddressWorkspaces));
            services.AddSingleton<IProcessBookingManager, ProcessBookingManager>(s => new ProcessBookingManager(AppSettings.Current.BaseAddressNotifications));

            _serviceProvider = services.BuildServiceProvider();

            MainWindow = _serviceProvider.GetService<MainWindow>();
            MainWindow.Show();
        }
    }
}
