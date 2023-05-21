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
    public class TenantController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        private readonly IConfiguration _configuration;
        private readonly TenantService _tenantService;
        private readonly ILogger _logger;
        private readonly UserService _userService;

        public TenantController(IConfiguration configuration, UserService userService, TenantService tenantService)
        {
            _configuration = configuration;
            _userService = userService;
            _tenantService = tenantService;
        }

        [HttpPost("addTenant")]
        [Authorize(Roles = DefaultString.ROLE_2)]
        public async Task<ActionResult> addTenant(NewTenant tenant)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _tenantService.newTenant(tenant);
            if (data == null)
            {
                return BadRequest("Thêm người thất bại");
            }
            return Ok(data);
        }

        [HttpGet("getTenantRoom")]
        [Authorize]
        public async Task<ActionResult> getTenantRoom(string idRoom,string? status)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _tenantService.getTenantByRoom(idRoom, status, email);
            if (data == null)
            {
                return BadRequest("Bạn không có quyền xem");
            }
            return Ok(data);
        }

        [HttpPost("delTenant")]
        [Authorize]
        public async Task<ActionResult> delTenant(delTenant del)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _tenantService.delTenant(del.tenantId, email);
            if (data == null)
            {
                return BadRequest("xóa người thất bại");
            }
            return Ok("Thành công");
        }


        [HttpPost("updateTenant")]
        [Authorize]
        public async Task<ActionResult> updateTenant(updateTenant update)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _tenantService.update(update, email);
            if (data == null)
            {
                return BadRequest("cập nhập thất bại");
            }
            return Ok(data);
        }


        [HttpGet("fetchTenant")]
        [Authorize]
        public async Task<ActionResult> fetchTenant(string cartId)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _tenantService.fetchTenant(cartId);
            return Ok(data);
        }

    }
}
