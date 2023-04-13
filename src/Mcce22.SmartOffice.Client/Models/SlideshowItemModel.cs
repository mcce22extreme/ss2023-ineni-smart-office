namespace Mcce22.SmartOffice.Client.Models
{
    public class SlideshowItemModel : IModel
    {
        public int Id { get; set; }

        public string ResourceUrl { get; set; }

        public string FileName { get; set; }

        public int UserId { get; set; }

        public bool HasContent { get; set; }
    }
}
