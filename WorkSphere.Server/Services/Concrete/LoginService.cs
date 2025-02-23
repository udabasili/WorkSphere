using WorkSphere.Server.Dtos;
using WorkSphere.Server.Repository;

namespace WorkSphere.Server.Services
{
    /// <inheritdoc />
    public class LoginService : ILoginService
    {
        private readonly ILoginRepo _repo;

        public LoginService(ILoginRepo loginRepo)
        {
            _repo = loginRepo;
        }


        public async Task<UserDto> Login(LoginDto user)
        {


            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
                return null;

            var userDto = await _repo.LoginAsync(user);
            if (userDto == null)
                return null;

            return userDto;

        }

    }
}
