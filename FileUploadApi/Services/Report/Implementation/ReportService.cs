using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.Report.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Report.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IRepository<LoginActivity> _loginActivity;
        private readonly IRepository<User> _user;
        public ReportService(IRepository<LoginActivity> loginActivity, IRepository<User> user)
        {
            _loginActivity = loginActivity;
            _user = user;
        }
        public async Task<IEnumerable<ReportModel>> Generate(string email)
        {
            var res = from l in await _loginActivity.GetAllAsync()
                      join u in await _user.GetAllAsync()
                      on l.UserId equals u.Id
                      where u.Email == email
                      select new ReportModel
                      {
                          FirstName = u.FirstName,
                          LastName = u.LastName,
                          LoginTime = l.LoginTime,
                          LogoutTime = l.LogoutTime

                      };
            var r = await _loginActivity.GetAllIncludeAsync(
                       x => new ReportModel {
                           FirstName = x.Users.FirstName,
                           LastName = x.Users.LastName,
                           LoginTime = x.LoginTime,
                           LogoutTime = x.LogoutTime
                           
                       },
                       x => x.Users.Email == email,
                       null,
                       null,
                       true
                );
            return r;
        }
    }
}
