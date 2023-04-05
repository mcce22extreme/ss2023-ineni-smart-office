namespace Mcce22.SmartOffice.Management.Models
{
    public class SlideshowItemModel
    {
        public int Id { get; set; }

        public string ResourceUrl { get; set; }

        public int UserId { get; set; }

        public bool HasContent { get; set; }
    }

    public class SaveSlideshowItemModel
    {
        public string FileName { get; set; }

        public int UserId { get; set; }
    }
}
