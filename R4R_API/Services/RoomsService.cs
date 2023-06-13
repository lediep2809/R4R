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
using R4R_API.ApiModel;
using R4R_API.Constant;
using System.Net;

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
            var address = paging.address.Trim();
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
                        && (address == "" || p.Address.Contains(address))
                        && (utilities == "" || p.utilities.Contains(util))
                        && (noSex == "" || p.noSex.Contains(noSex))
                        && (status =="" || p.Status.Equals(s)) )
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(s => s.Status)
                    .ToList();
            var total = _Db.Rooms
                    .FromSqlRaw($"select * from room as u where ( '{price}' = '' or TO_NUMBER(u.price,'9999999999') between '{to}' and '{from}')")
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search)
                        || p.Address.ToUpper().Trim().Contains(search)
                        || p.Area.ToUpper().Trim().Contains(search))
                        && (category == "" || p.Category.Equals(category))
                        && (address == "" || p.Address.Contains(address))
                        && (utilities == "" || p.utilities.Contains(util))
                        && (noSex == "" || p.noSex.Contains(noSex))
                        && (status == "" || p.Status.Equals(s))).Count();

            List<getAllRoom> allRooms = new List<getAllRoom>();

            foreach (var room in test)
            {
                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

                var imgRooms = _Db.ImgRooms
                .Where(m => m.idroom.Equals(room.Id))
                .Select(u => u.imgbase64)
                .FirstOrDefault();
                room.imgRoom = imgRooms;
               /* string[] foos = room.imgRoom.Split("(,)");*/

                string[] ulti = room.utilities.Split(",");
                allRoom.Utilities = ulti;
                allRooms.Add(allRoom);
                allRoom.total = total;
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
            var address = paging.address.Trim();
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
                        && (address == "" || p.Address.Contains(address))
                        && (utilities == "" || p.utilities.Contains(util))
                        && (noSex == "" || p.noSex.Contains(noSex))
                        && (status == "" || p.Status.Equals(s))
                        && (email == "" || p.Createdby.Equals(email)))
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(s => s.Createdby)
                    .ToList();

            var total = _Db.Rooms
                    .FromSqlRaw($"select * from room as u where ( '{price}' = '' or TO_NUMBER(u.price,'9999999999') between '{to}' and '{from}')")
                    .Where(p => (p.Name.ToUpper().Trim().Contains(search)
                        || p.Address.ToUpper().Trim().Contains(search)
                        || p.Area.ToUpper().Trim().Contains(search))
                        && (category == "" || p.Category.Equals(category))
                        && (address == "" || p.Address.Contains(address))
                        && (utilities == "" || p.utilities.Contains(util))
                        && (noSex == "" || p.noSex.Contains(noSex))
                        && (status == "" || p.Status.Equals(s))
                        && (email == "" || p.Createdby.Equals(email))).Count();

            List<getAllRoom> allRooms = new List<getAllRoom>();

            foreach (var room in test)
            {
                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

                var imgRooms = _Db.ImgRooms
                 .Where(m => m.idroom.Equals(room.Id))
                 .Select(u => u.imgbase64)
                 .FirstOrDefault();
                room.imgRoom = imgRooms;

                string[] ulti = room.utilities.Split(',');
                allRoom.Utilities = ulti;
                allRooms.Add(allRoom);
                allRoom.total = total;
            }


            return allRooms;
        }

        public getAllRoom GetRoomById(string id)
        {
            try
            {
                var room = _Db.Rooms
                    .Where(u => u.Id.Equals(id))
                    .FirstOrDefault();

                getAllRoom allRoom = new getAllRoom();
                allRoom.room = room;

                var imgRooms = _Db.ImgRooms
                .Where(m => m.idroom.Equals(room.Id))
                .Select(u => u.imgbase64)
                .ToList();
                allRoom.ImgRoom = imgRooms;

                string[] ulti = room.utilities.Trim().Split(',');
                allRoom.Utilities = ulti;
                int? view = room.view == null ? 0 : room.view;
                room.view = view + 1;

                _Db.Rooms.Update(room);
                _Db.SaveChanges();
                return allRoom;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Room saveRoom(SaveNewRoom newRoom, string[] img,string email,string role)
        {
            try
            {
                Room room = new Room();
                Guid uuid = Guid.NewGuid();
                room.Id = uuid.ToString();
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
                /*            var img = string.Join("(,)", newRoom.imgRoom);
                            room.imgRoom = img;*/
                room.noSex = room.noSex;

                var util = string.Join(',', newRoom.utilities);
                room.utilities = util.Trim();
                room.Createddate = DateTime.Today;
                room.Status = 1;

                foreach (var i in img)
                {
                    imgRoom ro = new imgRoom();
                    Category data = new Category();
                    Guid myuuid = Guid.NewGuid();
                    ro.Id = myuuid.ToString();
                    ro.idroom = room.Id;
                    ro.imgbase64 = i;
                    _Db.ImgRooms.Add(ro);
                }

                if (!DefaultString.ADMIN.Equals(role))
                {
                    User? user = _Db.Users.Where(e => e.Email.Equals(email)).FirstOrDefault();
                    if (user == null || user.bankBal == null || user.bankBal < 10000)
                    {
                        return null;
                    }
                    room.Status = 0;
                }


                _Db.Rooms.Add(room);
                _Db.SaveChanges();

                return room;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Room updateRoom(EditRoom room, string[] img,string emailEdit)
        {
            try
            {

                var roomCheck = _Db.Rooms.Where(e => e.Id.Equals(room.Id)).FirstOrDefault();

                if (roomCheck == null && emailEdit.Equals(roomCheck.Createdby))
                {
                    return null;
                }
                roomCheck.Name = room.Name;
                roomCheck.Address = room.Address;
                roomCheck.Category = room.Category;
                roomCheck.Area = room.Area;
                roomCheck.Capacity = room.Capacity;
                roomCheck.Description = room.Description;
                roomCheck.Price = room.Price;
                roomCheck.Deposit = room.Deposit;
                roomCheck.Electricprice = room.Electricprice;
                roomCheck.Waterprice = room.Waterprice;
                roomCheck.Otherprice = room.Otherprice;
                roomCheck.Houseowner = room.Houseowner;
                roomCheck.Ownerphone = room.Ownerphone;
                roomCheck.Status = room.Status;
                roomCheck.noSex = room.noSex;
                var util = string.Join(',', room.utilities);
                roomCheck.utilities = util.Trim();


                var imgRooms = _Db.ImgRooms
                    .Where(m => m.idroom.Equals(room.Id));
                _Db.ImgRooms.RemoveRange(imgRooms);
                

                foreach (var i in img)
                {
                    imgRoom ro = new imgRoom();
                    Category data = new Category();
                    Guid myuuid = Guid.NewGuid();
                    ro.Id = myuuid.ToString();
                    ro.idroom = room.Id;
                    ro.imgbase64 = i;
                    _Db.ImgRooms.Add(ro);
                }
                
                _Db.Rooms.Update(roomCheck);
                _Db.SaveChanges();

                return roomCheck;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Room updateRoom(activeRoom room, string emailEdit)
        {
            try
            {
                var roomCheck = _Db.Rooms.Where(e => e.Id == room.Id).FirstOrDefault();

                if (roomCheck == null)
                {
                    return null;
                }

                User? user = _Db.Users.Where(e => e.Email.Equals(roomCheck.Createdby)).FirstOrDefault();

                if (user == null || user.bankBal == null || user.bankBal < 100000)
                {
                    return null;
                }
                var money = user.bankBal;
                user.bankBal = money - 100000;
                _Db.Users.Update(user);

                roomCheck.Activeby = emailEdit;
                roomCheck.Activedate = DateTime.Today;
                roomCheck.Status = room.Status;

                hisRecharge his = new hisRecharge();
                his.Id = Guid.NewGuid().ToString();
                his.userEmail = user.Email;
                his.moneyRecharge = -100000;
                his.createDate = DateTime.Today;
                his.note = "Admin trừ tiền duyệt phòng";

                User? userAdmin = _Db.Users.Where(e => e.Roleid.Equals(DefaultString.ADMIN_CODE)).FirstOrDefault();

                if (userAdmin != null)
                {
                    userAdmin.bankBal = userAdmin.bankBal + 100000;
                    _Db.Users.Update(userAdmin);

                    hisRecharge hisAdmin = new hisRecharge();
                    hisAdmin.Id = Guid.NewGuid().ToString();
                    hisAdmin.userEmail = userAdmin.Email;
                    hisAdmin.moneyRecharge = 100000;
                    hisAdmin.createDate = DateTime.Today;
                    hisAdmin.note = "Admin cộng tiền duyệt phòng";
                    _Db.HisRecharges.Add(hisAdmin);
                }



                _Db.HisRecharges.Add(his);
                _Db.Rooms.Update(roomCheck);
                _Db.SaveChanges();

                return roomCheck;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}
