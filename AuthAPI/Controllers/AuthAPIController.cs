using Mango.MessageService;
using Mangoes.Services.AuthAPI.Model.DTO;
using Mangoes.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Mangoes.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private IAuthServices _authServices;
        private ResponseDTO _responseDTO;
        private IConfiguration _configuration;
        IMessageService _messageService;
        public AuthAPIController(IAuthServices authServices, IMessageService messageService, IConfiguration configuration)
        {
            _authServices = authServices;
            _responseDTO = new();
            _configuration = configuration;
            _messageService = messageService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDTO registerRequestDTO)
        {
           string result = await _authServices.Registration(registerRequestDTO);
            if (result.IsNullOrEmpty())
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Result = result;
                _responseDTO.Message = "Could not registered User";
                return BadRequest(_responseDTO);
            }
            else
            {
                string topicQueueName = _configuration.GetValue<string>("TopicsAndQueueNames:EmailNewUser");
                await _messageService.PublishMessage(result, topicQueueName);
                return Ok(_responseDTO);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO loginRequestDTO)
        {
            var result =  await _authServices.Login(loginRequestDTO);
            if(result.User==null)
            {
                _responseDTO.Result= result;
                _responseDTO.IsSuccess= false;
                return BadRequest(result);
            }
            _responseDTO.Result = result;
            return Ok(_responseDTO);
        }

        [HttpPost("Assignrole")]
        public async Task<IActionResult> AssignRole([FromBody]RegisterRequestDTO registerRequestDTO)
        {
            bool result = await _authServices.AssignRole(registerRequestDTO.Email, registerRequestDTO.Role);
            if (!result)
            {
                _responseDTO.Result = result;
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Could not registered User";
                return BadRequest(result);
            }
            return Ok(_responseDTO);
        }


    }
}
