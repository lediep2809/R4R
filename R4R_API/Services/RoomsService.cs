using R4R_API.Models;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;

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
            var search =paging.SearchQuery.Trim();
            var price = paging.Price.ToLower().Trim();
            var category = paging.Category.Trim();
            var utilities = paging.utilities.Trim();
            var noSex = paging.noSex.ToUpper().Trim();
            var status = paging.status.ToUpper().Trim();
            int? va = null;
            var to = 0;
            var from = 0;

            if (price.Equals("first"))
            {
                to = 1000000;
                from = 5000000;
            }
            else if (price.Equals("second"))
            {
                to = 6000000;
                from = 10000000;
            }
            else if (price.Equals("third"))
            {
                to = 11000000;
                from = 15000000;
            }

            var test = _Db.Rooms
                    .FromSqlRaw($"select * from room as u where ( '{price}' = '' or TO_NUMBER(u.price,'9999999999') between '{to}' and '{from}')")
                    .OrderBy(s => s.Status)
                    .ToList();

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

            foreach (var room in test)
            {
                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

                string[] foos = room.imgRoom.Split(",");
                string[] ulti = room.imgRoom.Split(",");
                allRoom.ImgRoom = foos;
                allRoom.Utilities = ulti;
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
