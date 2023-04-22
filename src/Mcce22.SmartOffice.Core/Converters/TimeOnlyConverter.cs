using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Mcce22.SmartOffice.Core.Converters
{
    public class TimeOnlyConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            var primitive = entry as Primitive;

            var timeSpan = TimeSpan.Parse(primitive.Value.ToString());
            return TimeOnly.FromTimeSpan(timeSpan);
        }

        public DynamoDBEntry ToEntry(object value)
        {
            var timeOnly = (TimeOnly)value;

            return timeOnly.ToLongTimeString();
        }
    }
}
