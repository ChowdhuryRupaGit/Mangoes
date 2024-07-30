using Newtonsoft.Json;
using System.Net;
using System.Text;
using WebAppMango.Models.DTO;
using static WebAppMango.Utilities.SD;

namespace WebAppMango.Services
{
    public class BusService : IBusService
    {
        public readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BusService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withbearer=true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient();
                HttpRequestMessage message = new ();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(requestDTO.Url);
                if (withbearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");

                }

                if (requestDTO.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;
                switch (requestDTO.APIType)
                {
                    case APITypes.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case APITypes.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case APITypes.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default: message.Method = HttpMethod.Get; break;
                }
                apiResponse = await client.SendAsync(message);

               
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDTO() { IsSuccess = false, Message = "Internal Server Error" };
                    case HttpStatusCode.NotFound:
                        return new ResponseDTO() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Unauthorized:
                         return new ResponseDTO() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDTO() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.OK:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResonseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                        return apiResonseDTO;
                        default : return new ResponseDTO() { IsSuccess = false, Message = "Not valid" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { IsSuccess = false, Message = ex.Message.ToString() };

            };
         
        }
    }
}
