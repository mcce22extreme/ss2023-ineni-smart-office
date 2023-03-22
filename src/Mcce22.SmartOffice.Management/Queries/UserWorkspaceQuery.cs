
using System.ComponentModel.DataAnnotations;

namespace Mcce22.SmartOffice.Management.Queries
{
    public class UserWorkspaceQuery
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int WorkspaceId { get; set; }
    }
}
