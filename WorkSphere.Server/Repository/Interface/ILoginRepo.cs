using WorkSphere.Server.Dtos;

namespace WorkSphere.Server.Repository
{

    public interface ILoginRepo
    {

        public Task<UserDto?> LoginAsync(LoginDto model);


    }
}
