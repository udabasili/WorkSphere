
using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Services;

public interface ITokenService
{
    string CreateToken(UserDto user);

    public Task<LoginOutputDto?> VerifyToken(string token);
}
