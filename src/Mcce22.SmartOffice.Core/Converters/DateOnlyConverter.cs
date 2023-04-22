using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Mcce22.SmartOffice.Core.Converters
{
    public class DateOnlyConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            var primitive = entry as Primitive;

            var dateTime = DateTime.Parse(primitive.Value.ToString());
            return DateOnly.FromDateTime(dateTime);
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var dateOnly = (DateOnly)value;

            return dateOnly.ToLongDateString();
        }
    }
}
