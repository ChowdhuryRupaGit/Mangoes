
using static WebAppMango.Utilities.SD;

namespace WebAppMango.Models.DTO
{
    public class RequestDTO
    {
        public APITypes APIType { get; set; } = APITypes.GET;
        public string Url { get; set; } = "";
        public string AccessToken { get; set; } = "";
        public object Data { get; set; } = "";
    }
}
