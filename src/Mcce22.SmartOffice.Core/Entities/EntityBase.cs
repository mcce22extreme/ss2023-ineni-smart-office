namespace Mcce22.SmartOffice.Core.Entities
{
    public interface IEntity
    {
        int Id { get; }
    }

    public abstract class EntityBase
    {
        public int Id { get; set; }
    }
}
