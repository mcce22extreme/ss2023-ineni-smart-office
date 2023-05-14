using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Mcce22.SmartOffice.Core.Converters
{
    public class TimeOnlyConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            var primitive = entry as Primitive;

            var dateTime = DateTime.Parse(primitive.Value.ToString());
            return TimeOnly.FromDateTime(dateTime);
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var timeOnly = (TimeOnly)value;

            return timeOnly.ToLongTimeString();
        }
    }
}
