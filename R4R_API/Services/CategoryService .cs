using R4R_API.Models;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using R4R_API.Constant;

namespace R4R_API.Services
{
    public class CategoryService
    {

        private readonly R4rContext _Db;

        public CategoryService(R4rContext Db)
        {
            _Db = Db;
        }

        public Category getbycode(string data)
        {
            try
            {
                Category category = _Db.Categories.Where(e => e.Code == data.Trim()).FirstOrDefault();

                return category;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Category saveCategory(Category data)
        {
            try
            {
                _Db.Categories.Add(data);
                _Db.SaveChanges();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Category updateCategory(Category data)
        {
            try
            {
                _Db.Categories.Update(data);
                _Db.SaveChanges();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
