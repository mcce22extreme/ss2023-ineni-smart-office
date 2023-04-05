using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class SlideshowItem : EntityBase
    {
        public string ResourceKey { get; set; }

        public User User { get; set; }

        public bool HasContent { get; set; }
    }
}
