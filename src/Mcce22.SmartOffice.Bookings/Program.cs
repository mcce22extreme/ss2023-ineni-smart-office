namespace Mcce22.SmartOffice.Bookings
{
    public class Program
    {
        private const string API_PREFIX = "bookingapi";

        public static async Task Main(string[] args)
        {
            await new Bootstrap().Run(args);
        }
    }
}
