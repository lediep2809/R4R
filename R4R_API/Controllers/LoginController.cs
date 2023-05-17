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
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Net.Http.Headers;

namespace AuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        public static User user = new User();
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("test")]
        [Authorize(Roles = DefaultString.ROLE_2)]
        public async Task<ActionResult> GetMyName()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            var students = await _context.Users.ToListAsync();
            var test = _context.Users.ToList();
            return Ok(students);
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
            user.Createddate = DateTime.Today;
            user.Phone = request.phone;
            user.bankBal = 100000;
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
            var checkUser = _context.Users.Where(e => e.Email == request.Email.Trim()).FirstOrDefault();

            if (checkUser == null )
            {
                return BadRequest("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password.Trim(), checkUser.Password))
            {
                return BadRequest("Wrong password.");
            }

            var token = CreateToken(checkUser);

            return Ok(token);
        }

        private UserModel CreateToken(User user)
        {
            UserModel _userData = new UserModel();
            Role role = _context.Roles.Where(e=>e.Id==user.Roleid).FirstOrDefault();

            string roles = string.Join(",", role != null ? role.Code : "");

            List<Claim> claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, "JWTServiceAccessToken"),
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Role, roles),
                new Claim("RoleName",role!=null? role.Name:""),
                new Claim("Email", user.Email)
            };

            _userData.UserMessage = "Login Success";

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx"));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["JWTAuthenticationServer"],
                _configuration["JWTServicePostmanClient"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1000),
                signingCredentials: signIn);

            _userData.Email = user.Email;
            _userData.Role = role != null ? role.Name : "";
            _userData.FullName = user.Fullname;
            _userData.phone = user.Phone;
            _userData.Money = user.bankBal;
            _userData.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return _userData;
        }



    }
}
