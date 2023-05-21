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
    public class TenantService
    {

        private readonly R4rContext _Db;

        public TenantService(R4rContext Db)
        {
            _Db = Db;
        }

        public NewTenant newTenant(NewTenant newTenant)
        {
            try
            {
                var tenantCheck = _Db.Tenants.Where(e => e.cartId.Equals(newTenant.cartId)).ToList();

                if (tenantCheck != null)
                {
                    return null;
                }

                Tenant tenant = new Tenant();
                tenant.Id = Guid.NewGuid().ToString();
                tenant.Name = newTenant.Name;
                tenant.adress = newTenant.adress;
                tenant.cartId = newTenant.cartId;
                tenant.phone = newTenant.phone;
                tenant.idRoom = newTenant.idRoom;
                tenant.dateJoin = DateTime.Today;
                tenant.status = 1;
                _Db.Tenants.Add(tenant);
                _Db.SaveChanges();

                return newTenant;
            }
            catch { 
                return null;
            }
        }

        public Tenant delTenant(string id)
        {
            try
            {
                var tenant = _Db.Tenants.Where(e => e.Id.Equals(id)).FirstOrDefault();

                if (tenant == null)
                {
                    return null;
                }

                tenant.status = 0;
                tenant.dateOut = DateTime.Today;
                _Db.Tenants.Add(tenant);
                _Db.SaveChanges();

                return tenant;
            }
            catch
            {
                return null;
            }
        }

        public Tenant fetchTenant(string cartId)
        {
            try
            {
                var tenant = _Db.Tenants.Where(e => e.cartId.Equals(cartId))
                    .OrderByDescending(s => s.dateJoin)
                    .Select(s => new Tenant
                    {
                        cartId = s.cartId,
                        adress = s.adress,
                        phone = s.phone,
                        Name = s.Name
                    }
                    )
                    .FirstOrDefault();

                if (tenant == null)
                {
                    return null;
                }

                return tenant;
            }
            catch
            {
                return null;
            }
        }



    }
}
