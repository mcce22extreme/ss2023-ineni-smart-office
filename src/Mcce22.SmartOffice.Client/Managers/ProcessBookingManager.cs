﻿using System.Net.Http;
using System.Threading.Tasks;

namespace Mcce22.SmartOffice.Client.Managers
{
    public interface IProcessBookingManager
    {
        Task ProcessBookings();
    }

    public class ProcessBookingManager : IProcessBookingManager
    {
        private static readonly HttpClient HttpClient = new();

        private readonly string _baseUrl;

        public ProcessBookingManager(string baseUrl)
        {
            _baseUrl = $"{baseUrl}/notify/";
        }

        public async Task ProcessBookings()
        {
            await HttpClient.PostAsync($"{_baseUrl}", new StringContent(string.Empty));
        }
    }
}
