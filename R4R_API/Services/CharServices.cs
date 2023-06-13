﻿using R4R_API.Constant;
using R4R_API.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace R4R_API.Services
{
    public class CharServices
    {
        private readonly R4rContext _Db;

        public CharServices(R4rContext Db)
        {
            _Db = Db;
        }

        public Dictionary<String, int> getchartRoom(string email,string role)
        {
            try
            {
                Dictionary<String, int> capitalCities = new Dictionary<String, int>();
                var countActive=0;
                var countInActive=0;
                var count=0;
                var countCancel = 0;
                if (role.Equals(DefaultString.ADMIN))
                {
                     countActive = countRoom(1);
                     countInActive = countRoom(0);
                     count = countRoom(3);
                    countCancel = countRoom(-1);
                }
                else
                {
                    countActive = countUser(1,email);
                    countInActive = countUser(0, email);
                    count = countUser(3, email);
                    countCancel = countUser(-1, email);
                }
                

                capitalCities.Add("roomActive",countActive);
                capitalCities.Add("roomInActive", countInActive);
                capitalCities.Add("roomTenant", count);
                capitalCities.Add("roomCancel", countCancel);
                return capitalCities;
            }
            catch (Exception ex) {
                return new Dictionary<String, int>(); ;
              }
        }

        public int countRoom(int status)
        {
            return _Db.Rooms.Where(e => e.Status.Equals(status)).Count();
        }

        public int countUser(int status,string email)
        {
            return _Db.Rooms.Where(e => e.Status.Equals(status) && e.Createdby.Equals(email)).Count();
        }


        public Dictionary<string, int> getChartMonth(string email, string role, int nam)
        {

            Dictionary<String, int> capitalCities = new Dictionary<String, int>();
            var quy1 = 0;
            var quy2 = 0;
            var quy3 = 0;
            var quy4 = 0;
            
            quy1 = getMoneyMonth(email,1, nam, role);
            quy2 = getMoneyMonth(email,2, nam, role);
            quy3 = getMoneyMonth(email,3, nam, role);
            quy4 = getMoneyMonth(email,4,nam, role);


                capitalCities.Add("Quý 1", quy1);
            capitalCities.Add("Quý 2", quy2);
            capitalCities.Add("Quý 3", quy3);
            capitalCities.Add("Quý 4", quy4);

            return capitalCities;
        }

        public int getMoneyMonth(string email,int quy,int nam,string role)
        {
            try
            {
                DateTime? start = new DateTime();
                DateTime? end = new DateTime();

                if (1 == quy)
                {
                    start = new DateTime(nam == 0 ? DateTime.Today.Year : nam, 1, 1);
                    end = new DateTime(nam == 0 ? DateTime.Today.Year : nam, 3, 30);
                }
                else if (2 == quy)
                {
                    start = new DateTime(nam == 0 ? DateTime.Today.Year : nam, 4, 1);
                    end = new DateTime(nam == 0 ? DateTime.Today.Year : nam, 6, 30);
                }
                else if (3 == quy)
                {
                    start = new DateTime(nam == 0 ? DateTime.Today.Year : nam, 7, 1);
                    end = new DateTime(nam == 0 ? DateTime.Today.Year : nam, 9, 30);
                }
                else if (4 == quy)
                {
                    start = new DateTime(nam == 0 ? DateTime.Today.Year : nam, 10, 1);
                    end = new DateTime(nam == 0 ? DateTime.Today.Year : nam, 12, 30);
                }


                var monet = from a in _Db.PayRooms.ToList()
                            join b in _Db.Rooms on a.IdRoom equals b.Id
                            where email.Equals(b.Createdby) &&
                            (a.datePay >= start && a.datePay <= end)
                            && a.status.Equals(1)
                            select new
                            {
                                datePay = a.datePay,
                                roomPrice = a.RoomPrice == null ? 0 : a.RoomPrice,
                                createdby = b.Createdby
                            };

                var val = monet.Sum(e => e.roomPrice);

                int moneyActive = 0;
                if (role.Equals(DefaultString.ADMIN))
                {
                    var depMoney = from a in _Db.HisRecharges.ToList()
                                   where email.Equals(a.userEmail) &&
                                   (a.createDate >= start && a.createDate <= end)
                                   select new
                                   {
                                       datePay = a.createDate,
                                       money = a.moneyRecharge == null ? 0 : a.moneyRecharge,
                                   };
                    
                    
                        moneyActive = (int)depMoney.Sum(e => e.money);
                    
                }

                return (int)(val == null  ? 0 : val + moneyActive);
            } catch (Exception ex)
            {
                return 0;
            }
            
        }



        public List<string> randomAdress()
        {
            var address = _Db.Rooms.OrderByDescending(e => e.view).Take(10)
                .Select(e => e.Address).ToList();
            List<string> result = new List<string>();
            foreach(var i in address){
                if (i != null && getMatch(i) != "" && result.Count()<=5 )
                {
                    result.Add(getMatch(i));
                }
            }
            return result;
        }

        public string getMatch(string i)
        {
            Regex filter = new Regex("(Quận).+,");
            var match = filter.Match(i.ToString());
            if (match.Success)
            {
                return match.Value;
            }
            return "";
        }

    }
}
