namespace Mcce22.SmartOffice.Core.Generators
{
    public interface IWeiGenerator
    {
        int GenerateWei(double temperature, double humidity, double co2Level);
    }
}
