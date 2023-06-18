namespace Mcce22.SmartOffice.Simulator.Messages
{
    public class DataIngressMessage
    {
        public string WorkspaceNumber { get; set; }

        public double Temperature { get; set; }

        public double NoiseLevel { get; set; }

        public double Co2Level { get; set; }
    }
}
