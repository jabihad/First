using AutoMapper;
using BergerMsfaApi.Extensions;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.Login.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Login.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly IRepository<LoginActivity> _loginActivity;
        private readonly IMapper _mapper;
        public LoginService(
            IRepository<LoginActivity> loginActivity,
            IMapper mapper
            )
        {
            _loginActivity = loginActivity;
            _mapper = mapper;
        }
        public async Task<LoginActivity> CreateLoginTimeAsync(string userId)
        {
            try
            {
                var loginActivityModel = new LoginActivityModel()
                {
                    UserId = userId,
                    LoginTime = DateTime.UtcNow,
                };
                //var loginActivity = _mapper.Map<LoginActivity>(loginActivityModel);
                var loginActivity = loginActivityModel.ToMap<LoginActivityModel, LoginActivity>();
                /*var loginActivity = new LoginActivity()
                {
                    UserId = userId,
                    LoginTime = DateTime.UtcNow
                };*/

                await _loginActivity.CreateAsync(loginActivity);

                return loginActivity;
            }
            catch (Exception ex)
            {
                Log.Error("Failed to create Login Time");
                throw ex;
            }
        }
        public async Task<LoginActivity> CreateLogoutTimeAsync(string userId)
        {
            try
            {
                var loginActivity = await _loginActivity.FindAsync(u => u.UserId == userId && u.LogoutTime == null);
                loginActivity.LogoutTime = DateTime.UtcNow;
                var res = await _loginActivity.UpdateAsync(loginActivity);
                return res; 
            }
            catch (Exception ex)
            {
                Log.Error("Failed to create Logout Time");
                throw ex;
            }
        }
    }
}