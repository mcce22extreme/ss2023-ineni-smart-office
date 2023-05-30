namespace Mcce22.SmartOffice.DeviceActivator.Managers
{
    public interface IDeviceManager
    {
        Task ActivateDevice(string activationCode);
    }
}
