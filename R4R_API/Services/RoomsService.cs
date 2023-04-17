using R4R_API.Models;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace R4R_API.Services
{
    public class RoomsService
    {

        private readonly R4rContext _Db;

        public RoomsService(R4rContext Db)
        {
            _Db = Db;
        }

        public List<Room> GetAll(Paging paging)
        {
            int pageNum = paging.PageNumber <=0 ? 1 : paging.PageNumber;
            int pageSize = paging.PageSize > 10 || paging.PageSize <= 0 ? 10 : paging.PageSize;
            var search =paging.SearchQuery.ToUpper().Trim();

            var a = _Db.Rooms
                .Where(p => p.Name.ToUpper().Trim().Contains(search) || p.Address.ToUpper().Trim().Contains(search) || p.Deposit.ToUpper().Trim().Contains(search))
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return a;
        }
    
}
}
