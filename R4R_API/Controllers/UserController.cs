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
    public class UserController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        public static User user = new User();
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
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

    }
}
