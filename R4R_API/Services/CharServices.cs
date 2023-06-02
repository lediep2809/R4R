using R4R_API.Constant;
using R4R_API.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;

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
                if (role.Equals(DefaultString.ROLE_1))
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


        public Dictionary<string, int> getChartMonth(string email, string role)
        {


            return new Dictionary<string, int>(); ;
        }

        public List<string> randomAdress()
        {
            var address = _Db.Rooms.OrderByDescending(e => e.view).Take(5)
                .Select(e => e.Address).ToList();
            return address;
        }
    }
}
