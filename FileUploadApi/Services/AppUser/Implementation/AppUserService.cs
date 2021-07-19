using Entities.Models;
using FileUploadApi.Repositories;
using FileUploadApi.Services.AppUser.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileUploadApi.Services.AppUser.Implementation
{
    public class AppUserService : IAppUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        public AppUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public string GetuserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //return _httpContext.HttpContext.User?.Id
        }

        public string GetuserName()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.Name);
        }
        public string GetuserEmail()
        {
            //return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.Email);
            return _httpContext.HttpContext.User?.Identity.Name;
        }
    }
}
