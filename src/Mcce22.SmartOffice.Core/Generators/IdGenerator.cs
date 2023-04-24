namespace Mcce22.SmartOffice.Core.Generators
{
    public interface IIdGenerator
    {
        string GenerateId(int charCount = 12);
    }

    public class IdGenerator : IIdGenerator
    {
        private static readonly Random _random = new Random();

        public string GenerateId(int charCount = 12)
        {
            // bitCount = characterCount * log (targetBase) / log(2)
            var bitCount = 6 * charCount;
            var byteCount = (int)Math.Ceiling(bitCount / 8f);
            byte[] buffer = new byte[byteCount];
            _random.NextBytes(buffer);

            string guid = Convert.ToBase64String(buffer);
            // Replace URL unfriendly characters
            guid = guid.Replace('+', '-').Replace('/', '_');
            // Trim characters to fit the count
            return guid.Substring(0, charCount);
        }
    }
}
