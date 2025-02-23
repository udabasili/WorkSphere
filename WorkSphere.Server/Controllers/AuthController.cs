
using Microsoft.AspNetCore.Mvc;
using TastyTreats.Model.Entities;
using TastyTreats.Types;
using WorkSphere.Server.Dtos;
using WorkSphere.Server.Services;

/**
 * This controller is responsible for handling login requests.
 */
namespace GetRealListingsMgr.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;

        public AuthController(ILoginService loginService, ITokenService tokenService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
        }

        // api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginOutputDto>> Login(LoginDto credentials)
        {
            try
            {
                List<ValidationError> errors = new();

                if (string.IsNullOrEmpty(credentials.Email))
                {
                    errors.Add(new ValidationError("Email is required", ErrorType.Model));
                }

                if (string.IsNullOrEmpty(credentials.Password))
                {
                    errors.Add(new ValidationError("Password is required", ErrorType.Model));
                }

                if (errors.Any())
                {
                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }

                UserDto user = await _loginService.Login(credentials);
                if (user == null)
                {
                    errors.Add(new ValidationError("Invalid credentials", ErrorType.Model));

                    return BadRequest(new
                    {
                        type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        title = "Bad Request",
                        status = 400,
                        errors,
                        traceId = HttpContext.TraceIdentifier
                    });
                }

                return new LoginOutputDto()
                {
                    User = user,
                    Token = _tokenService.CreateToken(user),
                    ExpiresIn = 7 * 24 * 60 * 60
                };
            }
            catch (Exception ex)
            {
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }

        // api/auth/verify-token
        [HttpGet("verify-token")]
        public async Task<ActionResult<LoginOutputDto>> VerifyToken()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Token is required" });
                }

                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }
                LoginOutputDto loginOutputDto = await _tokenService.VerifyToken(token);
                if (loginOutputDto == null)
                {
                    return null;
                }

                return Ok(loginOutputDto);


            }
            catch (Exception ex)
            {
                return ErrorHandling.HandleException(ex, HttpContext);
            }
        }
    }
}
