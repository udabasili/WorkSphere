
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkSphere.Data;
using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly WorkSphereDbContext _context;

        /// <summary>
        /// Initializes a new instance of the IConfiguration class.
        /// IConfiguration is a built-in interface in .NET Core that provides access to configuration settings.
        /// The Setting was dne in the Program.cs file.
        /// </summary>
        /// <param name="configuration"></param>
        public TokenService(IConfiguration configuration, WorkSphereDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        public string CreateToken(UserDto user)
        {
            // Check if Jwt:Key is null or empty
            string? jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key is not configured.");
            }

            //Create a new symmetric security key
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            //List of claims we will store in the token
            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Role ?? "")
            };

            //Create new credentials for signing the token
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Describe the token. What goes inside
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims), // subject is the claims identity
                Expires = DateTime.Now.AddDays(7), // token expires in 7 days
                SigningCredentials = creds // credentials for signing the tokens
            };

            //Create a new token handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //Create the token
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            //Write the token and return
            return tokenHandler.WriteToken(token);

        }

        public async Task<LoginOutputDto?> VerifyToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            string? jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("Jwt:Key is not configured.");

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(jwtKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, // Change if needed
                    ValidateAudience = false, // Change if needed
                    ValidateLifetime = true, // Ensure token is not expired
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // Validate and decode token
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    var email = principal.Identity?.Name ?? "";
                    var role = principal.FindFirst(ClaimTypes.Role)?.Value ?? "";
                    var username = await _context.Users.Where(u => u.Email == email).Select(u => u.UserName).FirstOrDefaultAsync();

                    return new LoginOutputDto
                    {
                        User = new UserDto
                        {
                            Email = email,
                            Role = role,
                            Username = username
                        },
                        Token = token,
                        ExpiresIn = (int)(jwtToken.ValidTo - DateTime.UtcNow).TotalSeconds // Expiry in seconds
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
            }

            return null; // Invalid or expired token
        }
    }
}
