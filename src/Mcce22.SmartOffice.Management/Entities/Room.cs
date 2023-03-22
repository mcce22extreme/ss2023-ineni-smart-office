using Mcce22.SmartOffice.Core.Entities;

namespace Mcce22.SmartOffice.Management.Entities
{
    public class Room : EntityBase
    {
        public string Number { get; set; }

        public List<Workspace> Workspaces { get; }
    }
}
