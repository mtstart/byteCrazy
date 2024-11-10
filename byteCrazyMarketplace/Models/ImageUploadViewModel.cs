using System.Web;

namespace byteCrazy.Models
{
    public class ImageUploadViewModel
    {
        public string productID { get; set; }
        public HttpPostedFileBase UploadedImage { get; set; }
    }
}
