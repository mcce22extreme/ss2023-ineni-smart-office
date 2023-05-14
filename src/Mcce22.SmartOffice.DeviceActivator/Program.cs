namespace Mcce22.SmartOffice.DeviceActivator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new Bootstrap().Run(args);
        }
    }
}
