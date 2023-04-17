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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Net.Http.Headers;

namespace AuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        private readonly IConfiguration _configuration;
        private readonly RoomsService _roomsService;

        public RoomsController(IConfiguration configuration, RoomsService roomsService)
        {
            _configuration = configuration;
            _roomsService = roomsService;
        }


        [HttpGet("test")]
/*        [Authorize(Roles = DefaultString.ROLE_2)]
*/        public async Task<ActionResult> GetAll(Paging paging)
        {
            return Ok(_roomsService.GetAll(paging));
        }

       
    }
}
