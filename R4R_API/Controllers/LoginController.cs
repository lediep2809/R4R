using R4R_API.ApiModel;
using R4R_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using R4R_API.Services;
using R4R_API.Constant;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public LoginController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMyName()
        {
            return Ok(_userService.GetMyName());

            //var userName = User?.Identity?.Name;
            //var roleClaims = User?.FindAll(ClaimTypes.Role);
            //var roles = roleClaims?.Select(c => c.Value).ToList();
            //var roles2 = User?.Claims
            //    .Where(c => c.Type == ClaimTypes.Role)
            //    .Select(c => c.Value)
            //    .ToList();
            //return Ok(new { userName, roles, roles2 });
        }


        [HttpPost("register")]
        public ActionResult<User> Register(UserRegisterModel request)
        {
            Role role = _context.Roles.Where(e => e.Code == DefaultString.ROLE_2).FirstOrDefault();

            User userCheck = _context.Users.Where(e => e.Email == request.Email).FirstOrDefault();

            if (userCheck != null)
            {
                return BadRequest(DefaultString.ERROR_STRING.DUP_EMAIL);
            }

            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(request.Password);
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            user.Id = myuuidAsString;
            user.Email = request.Email;
            user.Password = passwordHash;
            user.Fullname = request.FullName;
            user.Createddate = new DateTime();
            user.Phone = request.phone;
            if (role != null)
            {
                user.Roleid = role.Id;

            }
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserLogin request)
        {
            if (_context.Users.Where(e => e.Email == request.Email).FirstOrDefault() == null )
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        private string CreateToken(User user)
        {
            Role role = _context.Roles.Where(e=>e.Id==user.Roleid).FirstOrDefault();

                        List<Claim> claims = new List<Claim> {
            
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Role, role!=null?role.Code:""),
                new Claim("RoleName",role!=null? role.Name:""),
                new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


       
    }
}
