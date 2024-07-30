using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAppMango.Models.DTO;
using WebAppMango.Services;
using WebAppMango.Utilities;

namespace WebAppMango.Controllers
{
    public class AuthAPIController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthAPIController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }


        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() { Text=SD.RoleAdmin,Value= SD.RoleAdmin},
                new SelectListItem() { Text=SD.RoleCustomer,Value= SD.RoleCustomer},
            };
            ViewBag.RoleList = roleList;
            return View();
        }




        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            if(ModelState.IsValid )
            {
                ResponseDTO? responseDTO = await _authService.Login(loginRequestDTO);
                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(responseDTO.Result));
                    await SignInUser(loginResponseDTO);
                    _tokenProvider.SetToken(loginResponseDTO.Token);
                    return RedirectToAction("Index", "Home");
                }
            }
           
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
                ResponseDTO? responseDTO = await _authService.Registration(registerRequestDTO);
                if (responseDTO != null && responseDTO.IsSuccess)
                {
                    if (string.IsNullOrEmpty(registerRequestDTO.Role))
                    {
                        registerRequestDTO.Role = SD.RoleCustomer;

                    }
                    ResponseDTO? assignRole = await _authService.AssignRole(registerRequestDTO);
                    if (assignRole != null && assignRole.IsSuccess)
                    {
                        TempData["Success"] = "Registration Successfull";
                        return RedirectToAction(nameof(Login));
                    }
                }
                else
                {
                TempData["error"] = responseDTO?.Message;
                var roleList = new List<SelectListItem>()
                    {
                     new SelectListItem() { Text=SD.RoleAdmin,Value= SD.RoleAdmin},
                     new SelectListItem() { Text=SD.RoleCustomer,Value= SD.RoleCustomer},
                    };
                    ViewBag.RoleList = roleList;
                    return View(roleList);
                }
            return View(registerRequestDTO);
        }

        private  async Task SignInUser(LoginResponseDTO dto)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwt = handler.ReadJwtToken(dto.Token);
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value));
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(x => x.Type == "role").Value));
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

                var principle = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }   

           
        }
    }
 }
