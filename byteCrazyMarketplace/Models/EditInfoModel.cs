using System.Web;

namespace byteCrazy.Models
{
    // data model for getting user input
    public class EditInfoModel
    {
        public string description { get; set; }
        public string productID { get; set; }
        public string priceValue { get; set; }
        public string locationValue { get; set; }
        public string categoryValue { get; set; }
        public string imgUrl { get; set; }
        public string title { get; set; }
    }

    public class SaveLikeModel
    {
        public string productID { get; set; }
        public string likeList { get; set; }
        public string userID { get; set; }
    }
}
