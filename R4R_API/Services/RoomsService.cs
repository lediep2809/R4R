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
            int pageSize =  paging.PageSize <= 0 ? 10 : paging.PageSize;
            var search =paging.SearchQuery.ToUpper().Trim();
            var price = paging.Price.ToLower().Trim();
            var category = paging.Category.Trim();
            var util = string.Join(",", paging.utilities);
            var utilities = util;
            var noSex = paging.noSex.ToUpper().Trim();
            var status = paging.status;
            int s = 0;

            Int32.TryParse(status, out s);

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
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search)
                        || p.Address.ToUpper().Trim().Contains(search)
                        || p.Area.ToUpper().Trim().Contains(search))
                        && (category == "" || p.Category.Equals(category))
                        && (utilities == "" || p.utilities.Contains(util))
                        && (noSex == "" || p.noSex.Contains(noSex))
                        && (status =="" || p.Status.Equals(s)) )
                    /*.Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)*/
                    .OrderBy(s => s.Status)
                    .ToList();

            List<getAllRoom> allRooms = new List<getAllRoom>();

            foreach (var room in test)
            {
                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

/*                string[] foos = room.imgRoom.Split(",data:").Select(s =>
                {
                    return string.Join("data:", s);
                }).ToArray(); ;*/
                string[] foos = room.imgRoom.Split("(,)");
                string[] ulti = room.utilities.Split(",");
                allRoom.ImgRoom = foos;
                allRoom.Utilities = ulti;
                allRooms.Add(allRoom);
            }


            return allRooms;
        }



        public List<getAllRoom> getRoomsByUser(Paging paging,string email)
        {
            int pageNum = paging.PageNumber <= 0 ? 1 : paging.PageNumber;
            int pageSize = paging.PageSize <= 0 ? 10 : paging.PageSize;
            var search = paging.SearchQuery.ToUpper().Trim();
            var price = paging.Price.ToLower().Trim();
            var category = paging.Category.Trim();
            var util = string.Join(",", paging.utilities);
            var utilities = util;
            var noSex = paging.noSex.ToUpper().Trim();
            var status = paging.status;
            int s = 0;

            Int32.TryParse(status, out s);

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
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search)
                        || p.Address.ToUpper().Trim().Contains(search)
                        || p.Area.ToUpper().Trim().Contains(search))
                        && (category == "" || p.Category.Equals(category))
                        && (utilities == "" || p.utilities.Contains(util))
                        && (noSex == "" || p.noSex.Contains(noSex))
                        && (status == "" || p.Status.Equals(s))
                        && (email == "" || p.Createdby.Equals(email)))
                    /*.Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)*/
                    .OrderBy(s => s.Createdby)
                    .ToList();

            List<getAllRoom> allRooms = new List<getAllRoom>();

            foreach (var room in test)
            {
                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

                /*                string[] foos = room.imgRoom.Split(",data:").Select(s =>
                                {
                                    return string.Join("data:", s);
                                }).ToArray(); ;*/
                string[] foos = room.imgRoom.Split("(,)");
                string[] ulti = room.utilities.Split(",");
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
