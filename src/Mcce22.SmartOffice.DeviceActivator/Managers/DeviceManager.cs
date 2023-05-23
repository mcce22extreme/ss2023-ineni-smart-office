using System.Text;
using Amazon.DynamoDBv2.DataModel;
using Amazon.IotData;
using Amazon.IotData.Model;
using Mcce22.SmartOffice.Core.Exceptions;
using Mcce22.SmartOffice.DeviceActivator.Entities;
using Mcce22.SmartOffice.DeviceActivator.Models;
using Newtonsoft.Json;

namespace Mcce22.SmartOffice.DeviceActivator.Managers
{
    public interface IDeviceManager
    {
        Task ActivateDevice(string activationCode);
    }

    public class DeviceManager : IDeviceManager
    {
        private const string TOPIC = "mcce22-smart-office/activate";

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private readonly IDynamoDBContext _dbContext;
        private readonly IAmazonIotData _dataClient;

        public DeviceManager(IDynamoDBContext dbContext, IAmazonIotData dataClient)
        {
            _dbContext = dbContext;
            _dataClient = dataClient;
        }

        public async Task ActivateDevice(string activationCode)
        {
            await _semaphore.WaitAsync();

            try
            {
                var bookings = await _dbContext.QueryAsync<Booking>(activationCode,new DynamoDBOperationConfig
                {
                    IndexName = $"{nameof(Booking.ActivationCode)}-index"
                })
                .GetRemainingAsync();

                var booking = bookings.FirstOrDefault();

                if (booking == null)
                {
                    throw new NotFoundException($"Could not find booking for activationcode '{activationCode}'!");
                }

                var configurations = await _dbContext.QueryAsync<WorkspaceConfiguration>($"{booking.WorkspaceId}-{booking.UserId}",new DynamoDBOperationConfig
                {
                    IndexName = $"{nameof(WorkspaceConfiguration.WorkspaceUser)}-index"
                })
                .GetRemainingAsync();

                var userImages = await _dbContext.QueryAsync<UserImage>(booking.UserId, new DynamoDBOperationConfig
                {
                    IndexName = $"{nameof(UserImage.UserId)}-index"
                }).GetRemainingAsync();

                var model = new ActivateModel
                {
                    WorkspaceId = booking.WorkspaceId,
                    UserId = booking.UserId,
                    BookingId = booking.Id,
                    DeskHeight = configurations.FirstOrDefault()?.DeskHeight ?? 0,
                    UserImageUrl = userImages.FirstOrDefault()?.Url
                };

                await _dataClient.PublishAsync(new PublishRequest
                {
                    Topic = TOPIC,
                    Payload = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model)))
                });

                booking.Activated = true;

                await _dbContext.SaveAsync(booking);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
