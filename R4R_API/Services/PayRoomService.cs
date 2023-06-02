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
using AuthenticationAndAuthorization.Controllers;

namespace R4R_API.Services
{
    public class PayRoomService
    {

        private readonly R4rContext _Db;

        public PayRoomService(R4rContext Db)
        {
            _Db = Db;
        }

        public List<PayRoom> getPayRoom(string idRoom , string email)
        {
            try
            {
                var CheckRoom = _Db.Rooms.Where(e => e.Id.Equals(idRoom) && e.Createdby.Equals(email)).FirstOrDefault();

                if (CheckRoom == null)
                {
                    return null;
                }

                var Check = _Db.PayRooms.Where(e => e.IdRoom.Equals(idRoom))
                    .OrderByDescending(e=>e.Created).ToList();

               
                return Check;
            }
            catch
            {
                return null;
            }
        }

        public PayRoom NewPayMonth(NewPayMonth newPay, string email)
        {
            try
            {
                var CheckRoom = _Db.Rooms.Where(e => e.Id.Equals(newPay.IdRoom) && e.Createdby.Equals(email)).FirstOrDefault();
                
                if (CheckRoom == null)
                {
                    return null;
                }
                DateTime dateTime = new DateTime(newPay.Year == 0? DateTime.Today.Year : newPay.Year, newPay.Month,1);
                var Check = _Db.PayRooms.Where(e => e.IdRoom.Equals(newPay.IdRoom) 
                && e.datePay.Equals(dateTime)
                && e.status.Equals(1)).FirstOrDefault();

                var checkCartid = _Db.Tenants.Where(e => e.cartId.Equals(newPay.CartId) 
                && e.idRoom.Equals(newPay.IdRoom)).FirstOrDefault();

                if (Check != null || checkCartid == null)
                {
                    return null;
                }

                int ortherPrice = 0;
                int priceWater = 0;
                int price = 0;
                int priceElec = 0;

                Int32.TryParse(CheckRoom.Otherprice, out ortherPrice);
                Int32.TryParse(CheckRoom.Waterprice, out priceWater);
                Int32.TryParse(CheckRoom.Price, out price);
                Int32.TryParse(CheckRoom.Electricprice, out priceElec);

                PayRoom pay = new PayRoom();
                pay.Id = Guid.NewGuid().ToString();
                pay.IdRoom= newPay.IdRoom;
                pay.Month = newPay.Month;
                pay.Year = newPay.Year;
                pay.NoWater = newPay.NoWater;
                pay.NoElectic= newPay.NoElectic;
                pay.otherPrice = ortherPrice;
                pay.note= newPay.note ;
                pay.RoomPrice = price + priceElec * newPay.NoElectic + priceWater * newPay.NoWater + ortherPrice;
                pay.CartId = newPay.CartId;
                pay.Created = DateTime.Today;
                pay.status = newPay.status ;
                pay.datePay = dateTime;
                _Db.PayRooms.Add(pay);
                _Db.SaveChanges();

                return pay;
            }
            catch
            {
                return null;
            }
        }

        public PayRoom updateMonth(updatePay newPay, string email)
        {
            try
            {
                var CheckRoom = _Db.Rooms.Where(e => e.Id.Equals(newPay.IdRoom) 
                && e.Createdby.Equals(email)).FirstOrDefault();

                if (CheckRoom == null)
                {
                    return null;
                }

                var pay = _Db.PayRooms.Where(e => e.Id.Equals(newPay.id)).FirstOrDefault();

                if (pay == null )
                {
                    return null;
                }

                int ortherPrice = 0;
                int priceWater = 0;
                int price = 0;
                int priceElec = 0;

                Int32.TryParse(CheckRoom.Otherprice, out ortherPrice);
                Int32.TryParse(CheckRoom.Otherprice, out priceWater);
                Int32.TryParse(CheckRoom.Otherprice, out price);
                Int32.TryParse(CheckRoom.Otherprice, out priceElec);

                pay.NoWater = newPay.NoWater;
                pay.NoElectic = newPay.NoElectic;
                pay.otherPrice = ortherPrice;
                pay.note = newPay.note;
                pay.RoomPrice = price + priceElec * newPay.NoElectic + priceWater * newPay.NoWater + ortherPrice;
                pay.CartId = newPay.CartId;
                pay.Created = DateTime.Today;
                pay.status = newPay.status;

                _Db.PayRooms.Add(pay);
                _Db.SaveChanges();

                return pay;
            }
            catch
            {
                return null;
            }
        }

    }
}
