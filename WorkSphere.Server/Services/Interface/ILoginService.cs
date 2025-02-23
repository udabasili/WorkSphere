using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Services
{
    /// <summary>
    /// Defines methods for user authentication.
    /// </summary>
    public interface ILoginService
    {
        public Task<UserDto> Login(LoginDto user);

    }
}
