namespace Mcce22.SmartOffice.Workspaces
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new Bootstrap().Run(args);
        }
    }
}
