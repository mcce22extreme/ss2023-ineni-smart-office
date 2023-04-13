using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Users.Entities
{
    public class UserImage : EntityBase
    {
        public string ResourceKey { get; set; }

        public bool HasContent { get; set; }

        public int UserId { get; set; }
    }
}
