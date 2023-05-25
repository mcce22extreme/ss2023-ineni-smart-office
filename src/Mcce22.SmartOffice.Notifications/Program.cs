namespace Mcce22.SmartOffice.Notifications
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new Bootstrap().Run(args);           
        }
    }
}
