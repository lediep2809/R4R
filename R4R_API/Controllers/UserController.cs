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


        [HttpPost("getAllUser")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public ActionResult<User> getAllUser()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpPost("editUser")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> editUser(editUser user)
        {
            var checkUser = _context.Users.Where(e => e.Email == user.Email).FirstOrDefault();

            if (checkUser == null)
            {
                return BadRequest("Không tìm thấy user");
            }

            checkUser.Fullname = user.Fullname;


     /*       _context.Rooms.Remove(roomCheck);*/
            _context.SaveChanges();

            return Ok();
        }
    }
}
