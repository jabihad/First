using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Login.Interfaces
{
    public interface ILoginService
    {
        Task<LoginActivity> CreateLoginTimeAsync(string userId);
        Task<LoginActivity> CreateLogoutTimeAsync(string userId);
    }
}
