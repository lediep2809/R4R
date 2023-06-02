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
    public class ChartController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        private readonly IConfiguration _configuration;
        private readonly RoomsService _roomsService;
        private readonly CategoryService _categoryService;
        private readonly ILogger _logger;
        private readonly UserService _userService;
        private readonly CharServices _charServices;

        public ChartController(IConfiguration configuration, RoomsService roomsService, UserService userService, CharServices charServices)
        {
            _configuration = configuration;
            _roomsService = roomsService;
            _userService = userService;
            _charServices = charServices;
        }


        [HttpGet("getchart")]
        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var role = _userService.getTokenValue(Request, DefaultString.RoleName);
            return Ok(_charServices.getchartRoom(email, role));
        }

        [HttpGet("getRandomAdr")]
        public async Task<ActionResult> random()
        {
            return Ok(_charServices.randomAdress());
        }

        [HttpGet("GetChartMoney")]
        [Authorize]
        public async Task<ActionResult> GetChartMoney(int nam)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var role = _userService.getTokenValue(Request, DefaultString.RoleName);
            return Ok(_charServices.getChartMonth(email, role, nam));
        }
    }
}
