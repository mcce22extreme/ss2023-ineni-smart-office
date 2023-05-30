namespace Mcce22.SmartOffice.Core.Generators
{
    public interface IIdGenerator
    {
        string GenerateId(int charCount = 12);
    }
}
