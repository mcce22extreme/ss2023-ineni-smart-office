using Mcce22.SmartOffice.DataIngress.Entities;

namespace Mcce22.SmartOffice.DataIngress.Generators
{
    public class WeiGenerator : IWeiGenerator
    {
        private static readonly IDictionary<int, int> _temperatureMap = new Dictionary<int, int>()
        {
            { 30, 20 },
            { 28, 40 },
            { 26, 60 },
            { 24, 80 },
            { 22, 100 },
            { 20, 70 },
            { 18, 60 },
            { 16, 10 },
        };

        private static readonly IDictionary<int, int> _noiseMap = new Dictionary<int, int>()
        {
            { 60, 10 },
            { 55, 20 },
            { 50, 30 },
            { 45, 40 },
            { 40, 60 },
            { 30, 80 },
            { 25, 90 },
            { 20, 100 },
        };

        private static readonly IDictionary<int, int> _co2Map = new Dictionary<int, int>()
        {
            { 1000, 10 },
            { 900, 20 },
            { 850, 40 },
            { 800, 50 },
            { 750, 60 },
            { 700, 80 },
            { 650, 90 },
            { 600, 100 },
        };

        public int GenerateWei(WorkspaceData data)
        {
            var tempWei = GenerateTemperatureWei(data.Temperature);
            var noiseWei = GenerateNoiseWei(data.NoiseLevel);
            var co2Wei = GenerateCo2Wei(data.Co2Level);

            return (int)Math.Round((double)(tempWei + noiseWei + co2Wei) / 3);
        }

        private int GenerateTemperatureWei(double temperature)
        {
            var closestMatch = _temperatureMap.Keys.Aggregate((x, y) => Math.Abs(x - temperature) < Math.Abs(y - temperature) ? x : y);
            return _temperatureMap[closestMatch];
        }

        private int GenerateNoiseWei(double noiseLevel)
        {
            var closestMatch = _noiseMap.Keys.Aggregate((x, y) => Math.Abs(x - noiseLevel) < Math.Abs(y - noiseLevel) ? x : y);
            return _noiseMap[closestMatch];
        }

        private int GenerateCo2Wei(double co2Level)
        {
            var closestMatch = _co2Map.Keys.Aggregate((x, y) => Math.Abs(x - co2Level) < Math.Abs(y - co2Level) ? x : y);
            return _co2Map[closestMatch];
        }
    }
}
