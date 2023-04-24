using R4R_API.Models;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace R4R_API.Services
{
    public class RoomsService
    {

        private readonly R4rContext _Db;

        public RoomsService(R4rContext Db)
        {
            _Db = Db;
        }

        public List<getAllRoom> GetAll(Paging paging)
        {
            int pageNum = paging.PageNumber <=0 ? 1 : paging.PageNumber;
            int pageSize = paging.PageSize > 10 || paging.PageSize <= 0 ? 10 : paging.PageSize;
            var search =paging.SearchQuery.ToUpper().Trim();
            var price = paging.Price.ToUpper().Trim();
            var category = paging.Category.ToUpper().Trim();
            var utilities = paging.utilities.ToUpper().Trim();
            var noSex = paging.noSex.ToUpper().Trim();
            var status = paging.status.ToUpper().Trim();

            var a = _Db.Rooms
                .Where(p => (p.Name.ToUpper().Trim().Contains(search) 
                || p.Address.ToUpper().Trim().Contains(search) 
                || p.Deposit.ToUpper().Trim().Contains(search)) 
                && p.Category.Equals(category)
                && p.utilities.Contains(utilities)
                && p.noSex.Contains(noSex)
                && p.Status.Equals(status)
                )
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<getAllRoom> allRooms = new List<getAllRoom>();

            foreach (var room in a)
            {
                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

                string[] foos = room.imgRoom.Split(",");
                allRoom.ImgRoom = foos;
                allRooms.Add(allRoom);
            }


            return allRooms;
        }

        public Room saveRoom(Room room)
        {
            try
            {
                _Db.Rooms.Add(room);
                _Db.SaveChanges();

                return room;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Room updateRoom(Room room)
        {
            try
            {
                _Db.Rooms.Update(room);
                _Db.SaveChanges();

                return room;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
