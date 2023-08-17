using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using back_end.DTOs;
using back_end.Entities;
using back_end.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace back_end.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IDynamoDBContext _context;
        private readonly TokenService _tokenService;

        public AccountController(IDynamoDBContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _context.ScanAsync<User>(default).GetRemainingAsync();
            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var search = _context.ScanAsync<User>
            (
              new[] {
                new ScanCondition
                  (
                    "Username",
                    ScanOperator.Equal,
                    loginDto.Username
                  ),
                new ScanCondition
                  (
                    "Password",
                    ScanOperator.Equal,
                    loginDto.Password
                  )
              }
            );
            List<User> resultList = await search.GetRemainingAsync();

            if (resultList.Count <=0) return Unauthorized();
            User result = resultList.Last();

            return new UserDto
            {
                Username = result.Username,
                Token = _tokenService.GenerateToken(result)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(LoginDto registerDto)
        {

            var user = await _context.LoadAsync<User>(registerDto.Username);
            if (user != null) return BadRequest($"User with name {registerDto.Username} Already Exists");
            await _context.SaveAsync(new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = registerDto.Username,
                Password = registerDto.Password
            });
            return StatusCode(201);
        }
    }
}

