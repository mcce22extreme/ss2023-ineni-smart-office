using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Mcce22.SmartOffice.Core.Generators;
using Mcce22.SmartOffice.Notifications.Entities;
using Mcce22.SmartOffice.Notifications.Services;
using Serilog;

namespace Mcce22.SmartOffice.Notifications.Managers
{
    public class NotificationManager : INotificationManager
    {
        private static readonly SemaphoreSlim Semaphore = new (1);

        private readonly AmazonDynamoDBClient _dynamoDbClient;
        private readonly IEmailService _emailService;
        private readonly IIdGenerator _idGenerator;

        public NotificationManager(IEmailService emailService, IIdGenerator idGenerator)
        {
            _dynamoDbClient = new AmazonDynamoDBClient();
            _emailService = emailService;
            _idGenerator = idGenerator;
        }

        public async Task<int> ProcessPendingBookings()
        {
            await Semaphore.WaitAsync();

            try
            {
                using var context = new DynamoDBContext(_dynamoDbClient);

                var date = DateOnly.FromDateTime(DateTime.Now);

                var bookings = await context.QueryAsync<Booking>(
                date,
                new DynamoDBOperationConfig
                {
                    IndexName = $"{nameof(Booking.StartDate)}-index",
                })
                .GetRemainingAsync();

                var pendingBookings = bookings
                    .Where(x => !x.Activated && !x.InvitationSent)
                    .ToList();

                Log.Debug($"Found {pendingBookings.Count} pending booking(s).");

                foreach (var booking in pendingBookings)
                {
                    booking.ActivationCode = _idGenerator.GenerateId(20);
                    await _emailService.SendMail(booking);

                    booking.InvitationSent = true;
                    await context.SaveAsync(booking);
                }

                return pendingBookings.Count;
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}
