namespace WorkSphere.Server.Dtos
{
    public class LoginOutputDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
