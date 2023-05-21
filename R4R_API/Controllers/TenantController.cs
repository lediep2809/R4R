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

        [HttpPost("saveNewRoom")]
        [Authorize(Roles = DefaultString.ROLE_2)]
        public async Task<ActionResult> addTenant(NewTenant tenant)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var data = _tenantService.newTenant(tenant);
            if (data == null)
            {
                return BadRequest("Số dư đăng phòng không đủ");
            }
            return Ok(data);
        }
    }
}
