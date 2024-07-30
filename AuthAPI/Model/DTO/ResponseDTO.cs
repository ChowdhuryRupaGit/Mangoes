namespace Mangoes.Services.AuthAPI.Model.DTO
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object Result { get; set; } = string.Empty;
     
    }
}
