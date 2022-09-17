using PokemonAPI.AuthorizationAndAuthentication;
using PokemonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PokemonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly GenerateToken _generateToken;

        public LoginController(IConfiguration configuration, GenerateToken generateToken)
        {
            _configuration = configuration;
            _generateToken = generateToken;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PokemonModel>> Login([FromBody] Authenticate user)
        {
            var username = _configuration["UserAuthentication:login"];
            var password = _configuration["UserAuthentication:senha"];

            if (username != user.Username || password != user.Password)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            var validUser = new Users() { Username = username, Password = "" };
            var token = _generateToken.GenerateJwt(validUser);

            return Ok(new { user = validUser, token = token });
        }
    }
}