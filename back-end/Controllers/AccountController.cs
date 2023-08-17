using System.Text;
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

        /**
         * @description: just for test
         * @return {*}
         */
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
                    CreateMD5(loginDto.Password)
                  )
              }
            );
            List<User> resultList = await search.GetRemainingAsync();

            if (resultList.Count <= 0) return Unauthorized();
            User result = resultList.Last();

            return new UserDto
            {
                Username = result.Username,
                Token = _tokenService.GenerateToken(result)
            };
        }

        /**
         * @description: register a new user
         * @return {*}
         */
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(LoginDto registerDto)
        {

            var user = await _context.LoadAsync<User>(registerDto.Username);
            if (user != null) return BadRequest($"User with name {registerDto.Username} Already Exists");
            await _context.SaveAsync(new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = registerDto.Username,
                Password = CreateMD5(registerDto.Password)
            });
            return StatusCode(201);
        }

        /**
         * @description: create MD5 string for password encoding
         * @return {*}
         */
        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                //Convert the byte array to hexadecimal string
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}

