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
        private readonly UserService _userService;
        public UserController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


        [HttpPost("getAllUser")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public ActionResult<User> getAllUser()
        {
            return Ok(_context.Users.ToList());
        }

        [HttpPost("editUser")]
        [Authorize()]
        public async Task<ActionResult> editUser(editUser user)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email); 
                var role = _userService.getTokenValue(Request, DefaultString.RoleName); 
            var checkUser = _context.Users.Where(e => e.Email.Equals(user.Email)).FirstOrDefault();

            if (checkUser == null || email != checkUser.Email)
            {
                return BadRequest("Không tìm thấy user");
            }
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            checkUser.Fullname = user.Fullname;
            checkUser.Phone= user.Phone;
            checkUser.Password = passwordHash;
            checkUser.Status = user.Status;
            checkUser.Roleid = user.Roleid;

            _context.Users.Update(checkUser);
            _context.SaveChanges();

            return Ok(checkUser);
        }



        [HttpPost("deleteUser")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> deleteUser(deleteUser user)
        {
 /*           var email = _userService.getTokenValue(Request, DefaultString.Email);*/

            var Check = _context.Users.Where(e => e.Id.Equals(user.Id)).FirstOrDefault();

            if (Check == null)
            {
                return BadRequest("Không tìm thấy User");
            }

            _context.Users.Remove(Check);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("depMoney")]
        [Authorize()]
        public async Task<ActionResult> depMoney(depMoney money)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var role = _userService.getTokenValue(Request, DefaultString.RoleName);

            var checkUser = _context.Users.Where(e => e.Email.Equals(email)).FirstOrDefault();

            if (checkUser == null)
            {
                return BadRequest("Không tìm thấy user");
            }
            var moneyU = checkUser.bankBal == null ? 0 : checkUser.bankBal;
            checkUser.bankBal = moneyU + money.money;
            _context.Users.Update(checkUser);
            _context.SaveChanges();

            return Ok(checkUser);
        }
    }
}
