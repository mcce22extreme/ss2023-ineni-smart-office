namespace Mcce22.SmartOffice.Users
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await new Bootstrap().Run(args);
        }
    }
}
