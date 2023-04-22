using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Mcce22.SmartOffice.Client
{
    public class AppSettings
    {
        static AppSettings()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true)
                .Build();

            Current = Config.Get<AppSettings>();
        }

        public static AppSettings Current { get; }

        public static IConfigurationRoot Config { get; }

        private string _baseAddress;

        public string BaseAddress
        {
            get { return _baseAddress; }
            set
            {
                _baseAddress = value;
                if (!string.IsNullOrEmpty(_baseAddress))
                {
                    BaseAddressUsers = $"{_baseAddress}/userapi";
                    BaseAddressWorkspaces = $"{_baseAddress}/workspaceapi";
                    BaseAddressBookings = $"{_baseAddress}/bookingapi";
                }
            }
        }


        public string BaseAddressUsers { get; set; }

        public string BaseAddressWorkspaces { get; set; }

        public string BaseAddressBookings { get; set; }
    }
}
