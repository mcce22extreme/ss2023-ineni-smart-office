namespace Mcce22.SmartOffice.Client.Models
{
    public class UserImageModel : IModel
    {
        public string Id { get; set; }

        public string ResourceUrl { get; set; }

        public string FileName { get; set; }

        public string UserId { get; set; }

        public bool HasContent { get; set; }
    }
}
