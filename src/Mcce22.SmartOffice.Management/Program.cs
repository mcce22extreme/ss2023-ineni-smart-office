using Mcce22.SmartOffice.Management.Common;

namespace Mcce22.SmartOffice.Management
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new Bootstrap().Run(args);
        }
    }
}
