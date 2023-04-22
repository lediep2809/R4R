﻿using R4R_API.ApiModel;
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
        private readonly ILogger _logger;


        public RoomsController(IConfiguration configuration, RoomsService roomsService)
        {
            _configuration = configuration;
            _roomsService = roomsService;
        }


        [HttpGet("searchRooms")]
/*        [Authorize(Roles = DefaultString.ROLE_2)]
*/        public async Task<ActionResult> GetAll(Paging paging)
        {
            return Ok(_roomsService.GetAll(paging));
        }


        [HttpPost("saveNewRoom")]
        [Authorize]
        public async Task<ActionResult> saveRoom(SaveNewRoom newRoom)
        {
           
            var re = Request;
            var headers = re.Headers;
            string tokenString = headers.Authorization;
            var jwtEncodedString = tokenString.Substring(7); // trim 'Bearer ' from the start since its just a prefix for the token string
            var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);
            var email = token.Claims.First(c => c.Type == "Email").Value;
            

            Room room = new Room();
            Guid myuuid = Guid.NewGuid();
            room.Id = myuuid.ToString();
            room.Name = newRoom.Name;
            room.Address = newRoom.Address;
            room.Category = newRoom.Category;
            room.Area = newRoom.Area;
            room.Capacity = newRoom.Capacity;
            room.Description = newRoom.Description;
            room.Price = newRoom.Price;
            room.Deposit = newRoom.Deposit;
            room.Electricprice = newRoom.Electricprice;
            room.Waterprice = newRoom.Waterprice;
            room.Otherprice = newRoom.Otherprice;
            room.Houseowner = newRoom.Houseowner;
            room.Ownerphone = newRoom.Ownerphone;
            room.Createdby = email;
            room.imgRoom = newRoom.imgRoom;
            room.Createddate = new DateTime();
            room.Status = 0;

            return Ok(_roomsService.saveRoom(room));
        }


    }
}
