using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.AppUser.Interfaces
{
    public interface IAppUserService
    {
        string GetuserId();
        public string GetuserName();
        public string GetuserEmail();
    }
}
