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
    public class RoomsController : ControllerBase
    {
        R4rContext _context = new R4rContext();

        private readonly IConfiguration _configuration;
        private readonly RoomsService _roomsService;
        private readonly CategoryService _categoryService;
        private readonly ILogger _logger;
        private readonly UserService _userService;

        public RoomsController(IConfiguration configuration, RoomsService roomsService, UserService userService)
        {
            _configuration = configuration;
            _roomsService = roomsService;
            _userService = userService;
        }


        [HttpPost("searchRooms")]
        public async Task<ActionResult> GetAll(Paging paging)
        {
            return Ok(_roomsService.GetAll(paging));
        }

        [HttpGet("getRoomById")]
        public async Task<ActionResult> getOrderDetail(string id)
        {
            var alert = _roomsService.GetRoomById(id);
            if (alert != null)
            {
                return Ok(alert);
            }
            return BadRequest("Không tìm thấy giao dịch");
        }

        [HttpPost("getRoomsByUser")]
        [Authorize]
        public async Task<ActionResult> getRoomsByUser(Paging paging)
        {
            var re = Request;
            var headers = re.Headers;
            string tokenString = headers.Authorization;
            var jwtEncodedString = tokenString.Substring(7); // trim 'Bearer ' from the start since its just a prefix for the token string
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            var email = token.Claims.First(c => c.Type == "Email").Value;

            return Ok(_roomsService.getRoomsByUser(paging,email));
        }

        [HttpPost("getCategory")]
        public async Task<ActionResult> GetCategory()
        {
            var data = await _context.Categories.ToListAsync();
            return Ok(data);
        }

        [HttpPost("newCategory")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> newCategory(NewCategory category)
        {
            Category data = new Category();
            Guid myuuid = Guid.NewGuid();
            data.Id = myuuid.ToString();
            data.Code = category.Code;
            data.Name = category.Name;
            data.Status = "1";

            _context.Categories.Add(data);
            _context.SaveChanges();

          /*  _categoryService.saveCategory(data);*/
            return Ok(category);
        }

        [HttpPost("editCategory")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> editCategory(editCategory category)
        {
            var check =  _categoryService.getbycode(category.Code);

            if (check == null )
            {
                return BadRequest("Không tìm thấy Loại");
            }

            check.Name = category.Name;
            check.Status = "1".Equals(category.Status)? "1" : "0";
            try
            {
                _context.Categories.Update(check);
                _context.SaveChanges();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest("Không tìm thấy Loại");
            }
            
        }

        [HttpPost("deleteRoom")]
        [Authorize]
        public async Task<ActionResult> deleteRoom(activeRoom room)
        {

            var roomCheck = _context.Rooms.Where(e => e.Id == room.Id).FirstOrDefault();

            if (roomCheck == null)
            {
                return BadRequest("Không tìm thấy phòng");
            }

            _context.Rooms.Remove(roomCheck);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("editRooms")]
        [Authorize]
        public async Task<ActionResult> editRooms(EditRoom room)
        {

            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var roomEdit = _roomsService.updateRoom(room, room.imgRoom, email);
            if (roomEdit == null)
            {
                return BadRequest("Không tìm thấy phòng");
            }

            return Ok(roomEdit);
        }

        [HttpPost("activeRoom")]
        [Authorize(Roles = DefaultString.ROLE_1)]
        public async Task<ActionResult> activeRoom(activeRoom room)
        {

            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var roomEdit = _roomsService.updateRoom(room, email);
            if (roomEdit == null)
            {
                return BadRequest("Không tìm thấy phòng");
            }

            return Ok(roomEdit);
        }


        [HttpPost("saveNewRoom")]
        [Authorize]
        public async Task<ActionResult> saveRoom(SaveNewRoom newRoom)
        {
            var email = _userService.getTokenValue(Request, DefaultString.Email);
            var role = _userService.getTokenValue(Request, DefaultString.RoleName);

            var roomNew = _roomsService.saveRoom(newRoom, newRoom.imgRoom, email, role);
            if (roomNew == null)
            {
                return BadRequest("Số dư đăng phòng không đủ");
            }
            return Ok(roomNew);
        }

    }
}
