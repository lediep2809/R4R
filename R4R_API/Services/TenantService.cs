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
using Microsoft.IdentityModel.Tokens;

namespace R4R_API.Services
{
    public class TenantService
    {

        private readonly R4rContext _Db;

        public TenantService(R4rContext Db)
        {
            _Db = Db;
        }

        public NewTenant newTenant(NewTenant newTenant, string email)
        {
            try
            {
                var CheckRoom = _Db.Rooms.Where(e => e.Id.Equals(newTenant.idRoom) && e.Createdby.Equals(email)).FirstOrDefault();

                if (CheckRoom == null)
                {
                    return null;
                }


                var tenantCheck = _Db.Tenants.Where(e => e.cartId.Equals(newTenant.cartId) && e.idRoom.Equals(newTenant.idRoom)).FirstOrDefault();

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

                CheckRoom.Status =3;
                _Db.Rooms.Update(CheckRoom);
                _Db.Tenants.Add(tenant);
                _Db.SaveChanges();

                return newTenant;
            }
            catch { 
                return null;
            }
        }

        public List<Tenant> getTenantByRoom(string idRoom,string status,string email)
        {
            try
            {
                
                int s = 0;
                Int32.TryParse(status, out s);

                var check = _Db.Rooms.Where(e => e.Id.Equals(idRoom) && e.Createdby.Equals(email)).FirstOrDefault();
                if (check == null)
                {
                    return null;
                }

                var tenant = _Db.Tenants.Where(e => e.idRoom.Equals(idRoom) 
                && (status.IsNullOrEmpty() || e.status.Equals(s)) ).ToList();

                return tenant;
            }
            catch
            {
                return null;
            }
        }

        public Tenant delTenant(string tenantId, string email)
        {
            try
            {
                var tenant = _Db.Tenants.Where(e => e.Id.Equals(tenantId)).FirstOrDefault();

                if (tenant == null)
                {
                    return null;
                }

                var check = _Db.Rooms.Where(e => e.Id.Equals(tenant.idRoom) && e.Createdby.Equals(email)).FirstOrDefault();
                if (check == null)
                {
                    return null;
                }


                tenant.status = 0;
                tenant.dateOut = DateTime.Today;
                _Db.Tenants.Update(tenant);
                _Db.SaveChanges();

                var countT = _Db.Tenants.Where(e => e.idRoom.Equals(check.imgRoom) && e.status.Equals(1) ).Count();

                if(countT == 0) {
                    check.Status = 1;
                }
                _Db.Rooms.Update(check);
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

        public Tenant update(updateTenant update, string email)
        {
            try
            {
                var tenant = _Db.Tenants.Where(e => e.Id.Equals(update.idTenant)).FirstOrDefault();

                if (tenant == null)
                {
                    return null;
                }

                var check = _Db.Rooms.Where(e => e.Id.Equals(tenant.idRoom) && e.Createdby.Equals(email)).FirstOrDefault();
                if (check == null)
                {
                    return null;
                }



                tenant.Name = update.Name;
                tenant.adress = update.adress;
                tenant.cartId = update.cartId;
                tenant.phone = update.phone;

                if (!tenant.status.Equals(update.status))
                {
                    if (update.status.Equals(0))
                    {
                        tenant.dateOut = DateTime.Today;
                        tenant.status = update.status;
                        _Db.Tenants.Update(tenant);
                        _Db.SaveChanges();
                        var countT = _Db.Tenants.Where(e => e.idRoom.Equals(check.imgRoom) && e.status.Equals(1)).Count();

                        if (countT == 0)
                        {
                            check.Status = 1;
                        }
                        _Db.Rooms.Update(check);
                        _Db.SaveChanges();
                    }
                    if (update.status.Equals(1))
                    {
                        tenant.dateJoin = DateTime.Today;
                        tenant.status = update.status;
                        _Db.Tenants.Update(tenant);

                        check.Status = 3;
                        _Db.Rooms.Update(check);

                        _Db.SaveChanges();


                    }
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
