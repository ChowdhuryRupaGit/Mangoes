using Mangoes.Services.AuthAPI.Model;
using Mangoes.Services.AuthAPI.Model.DTO;
using Mangoes.Services.CouponAPIS.Data;
using Microsoft.AspNetCore.Identity;

namespace Mangoes.Services.AuthAPI.Services.IServices
{
    public class AuthServices : IAuthServices
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly AppDBContext _appDBContext;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly IJwtGenerator _jwtGenerator;

        public AuthServices(UserManager<ApplicationUser> userManager, AppDBContext appDBContext, RoleManager<IdentityRole> roleManager,
            IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _appDBContext = appDBContext;
            _roleManager = roleManager;
            _jwtGenerator= jwtGenerator;

        }

        public async Task<bool> AssignRole(string emailId, string role)
        {
            var user = _appDBContext.ApplicationUsers.FirstOrDefault(x=> x.Email.ToLower() ==emailId.ToLower());
            if(user!=null)
            {
                if(!_roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
               await _userManager.AddToRoleAsync(user, role);
                return true;
            }
            return false;
        } 

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _appDBContext.ApplicationUsers.FirstOrDefault(x=>x.UserName.ToLower()  == loginRequestDTO.UserName.ToLower());
            LoginResponseDTO responseDTO = new LoginResponseDTO();
            if (user != null)
            {
               bool validateUser = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if(validateUser)
                {
                    IEnumerable<string> roles =await _userManager.GetRolesAsync(user);
                    //Generate Token
                    string token = _jwtGenerator.GenerateJwtToken(user, roles);

                    UserDTO userDTO = new()
                    {
                        ID = user.Id,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email
                    };
                    responseDTO.User = userDTO;
                    responseDTO.Token = token;
                    return responseDTO;
                }
            }
            responseDTO.User = null;
            responseDTO.Token = "";
            return responseDTO;
        }

        public async Task<string> Registration(RegisterRequestDTO registerRequestDTO)
        {
            ApplicationUser user = new ApplicationUser
            {
                Name = registerRequestDTO.Name,
                Email = registerRequestDTO.Email,
                UserName = registerRequestDTO.Email,
                PhoneNumber = registerRequestDTO.PhoneNumber
            };
            try
            {
                var result = await _userManager.CreateAsync(user,registerRequestDTO.Password);
                if(result.Succeeded)
                {
                    var registeredUser = _appDBContext.ApplicationUsers.FirstOrDefault(x=>x.UserName == registerRequestDTO.Email);
                    UserDTO userDTO = new()
                    {
                        ID = registeredUser.Id,
                        Name = registeredUser.Name,
                        PhoneNumber = registeredUser.PhoneNumber,
                        Email = registeredUser.Email
                    };
                    return userDTO.Email;

                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
