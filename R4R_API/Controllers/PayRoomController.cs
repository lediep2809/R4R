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
using System.Net;
using System.Linq;
using System.IdentityModel.Tokens;

namespace AuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayRoomController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        private readonly IConfiguration _configuration;
        private readonly PayRoomService _payRoomService;
        private readonly ILogger _logger;
        private readonly UserService _userService;

        public PayRoomController(IConfiguration configuration, UserService userService, PayRoomService payRoomService)
        {
            _configuration = configuration;
            _userService = userService;
            _payRoomService = payRoomService;
        }

        [HttpPost("newPayRoom")]
        [Authorize]
        public async Task<ActionResult> newPayRoom(NewPayMonth newpay)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _payRoomService.NewPayMonth(newpay,email);
            if (data == null)
            {
                return BadRequest("thất bại");
            }
            return Ok(data);
        }

        [HttpGet("getPayMonthRoom")]
        [Authorize]
        public async Task<ActionResult> getPayMonthRoom(string idRoom)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _payRoomService.getPayRoom(idRoom, email);
            if (data == null)
            {
                return BadRequest("Bạn không có quyền xem");
            }
            return Ok(data);
        }

        [HttpPost("updatePayRoom")]
        [Authorize]
        public async Task<ActionResult> updatePayRoom(NewPayMonth update)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _payRoomService.updateMonth(update, email);
            if (data == null)
            {
                return BadRequest("update thất bại");
            }
            return Ok("Thành công");
        }

    }
}
