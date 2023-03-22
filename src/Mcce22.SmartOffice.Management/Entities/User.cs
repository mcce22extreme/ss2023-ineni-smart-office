using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class User : EntityBase
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<UserWorkspace> UserWorkspaces { get; set; }
    }
}
