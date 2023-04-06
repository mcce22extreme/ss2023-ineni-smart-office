namespace Mcce22.SmartOffice.Management.Queries
{
    public class CheckAvailabilityQuery
    {
        public int WorkspaceId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
