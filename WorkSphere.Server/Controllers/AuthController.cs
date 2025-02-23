
using Microsoft.AspNetCore.Mvc;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Services;

/**
 * This controller is responsible for handling login requests.
 */
namespace GetRealListingsMgr.API.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;


        public AuthController(ILoginService loginService, ITokenService tokenService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
        }


        [HttpPost]
        public async Task<ActionResult<LoginOutputDto>> Login(LoginDto credentials)
        {
            UserDto user = await _loginService.Login(credentials);

            if (user == null)
                return Unauthorized("Invalid login");

            return new LoginOutputDto()
            {
                User = user,
                Token = _tokenService.CreateToken(user),
                ExpiresIn = 7 * 24 * 60 * 60
            };
        }
    }
}
