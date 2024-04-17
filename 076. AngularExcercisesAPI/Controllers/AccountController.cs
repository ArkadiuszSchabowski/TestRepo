using _076.AngularExcercisesAPI.Models;
using _076.AngularExcercisesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace _076._AngularExcercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }
        [HttpPost("register")]
        public ActionResult CreateAccount([FromBody] CreateUserDto dto)
        {
            _service.CreateAccount(dto);
            return Ok();
        }
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginDto dto)
        {
            string token = _service.Login(dto);
            var serializedToken = JsonSerializer.Serialize(token);
            return Ok(serializedToken);
        }
    }
}
