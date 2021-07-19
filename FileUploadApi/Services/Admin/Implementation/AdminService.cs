using AutoMapper;
using Entities.DTO;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.Admin.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Admin.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<User> _user;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<Extension> _extension;
        private readonly IRepository<File> _file;
        private readonly IMapper _mapper;
        public AdminService(IRepository<User> user, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IRepository<Extension> extension, IRepository<File> file, IMapper mapper)
        {
            _user = user;
            _userManager = userManager;
            _roleManager = roleManager;
            _extension = extension;
            _file = file;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Extension>> GetExtension()
        {
            var res = await _extension.GetAllIncludeAsync(
                                 x => x,
                                 null,
                                 x => x.OrderBy(y => y.ExtensionName),
                                 null,
                                 true
                                 );
            return res;
        }
        public async Task<bool> CreateExtension(ExtensionModel extensionModel)
        {
            var res = _mapper.Map<Extension>(extensionModel);
            var chk = await IsExist(extensionModel.ExtensionName);
            if (!chk)
            {
                await _extension.CreateAsync(res);
                return true;
            }
            return false;
        }
        private async Task<bool> IsExist(string extension)
        {
            var ext = await _extension.FindAsync(e => e.ExtensionName.ToLower() == extension.ToLower());
            return ext == null ? false : true;
        }
        public async Task<ExtensionModel> GetExtensionById(int id)
        {
            var extension = await _extension.FindAsync(f => f.Id == id);
            var res = _mapper.Map<ExtensionModel>(extension);
            return res;
        }
        public async Task<bool> EditExtension(ExtensionModel extensionModel)
        {
            var ext = await _extension.FindAsync(e => e.Id == extensionModel.Id);
            ext.ExtensionName = extensionModel.ExtensionName;
            ext.MaxSize = (double)extensionModel.MaxSize;

            await _extension.UpdateAsync(ext);
            return true;
        }
        public async Task<int> DeleteExtension(ExtensionModel extensionModel)
        {
            var id = extensionModel.Id;
            return await _extension.DeleteAsync(e => e.Id == id);
        }
        public async Task<IEnumerable<FileModel>> FileList()
        {
            var file = await _file.GetAllAsync();
            var fileModel = _mapper.Map<IEnumerable<FileModel>>(file);
            return fileModel;
        }
        public async Task<bool> DeleteFile(FileModel fileModel)
        {
            var id = fileModel.Id;
            var res = await _file.DeleteAsync(f => f.Id == id);
            return true;
        }
        public async Task<bool> CreateUser(UserModel userModel)
        {
            try
            {
                var userForRegistrationDto = new UserForRegistrationDto()
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Email = userModel.Email
                };
                var user = _mapper.Map<User>(userForRegistrationDto);
                var result = await _userManager.CreateAsync(user, userModel.Password);
                if (!result.Succeeded)
                {
                    return false;
                }
                else
                {
                    var res = await _userManager.AddToRoleAsync(user, userModel.Role);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IEnumerable<UserModel>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userModel = _mapper.Map<IEnumerable<UserModel>>(users);
            //var user = await _userManager.FindByIdAsync(UserId);
            foreach (var user in userModel)
            {
                var u = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(u);
                user.Role = roles[0];
            }

            return userModel;
        }
        public async Task<UserModel> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }

        public async Task<bool> DeleteUser(UserModel userModel)
        {
            if (userModel.Role == "admin")
                return false;
            var user = await _userManager.FindByIdAsync(userModel.Id);

            var res = await _userManager.DeleteAsync(user);
            if (res.Succeeded)
                return true;
            return false;
        }
        public IEnumerable<IdentityRole> GetAllRoles()
        {
            var res = _roleManager.Roles.ToList();
            return res;
        }
        public async Task<bool> UpdateUser(UserModel userModel)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userModel.Id);
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.Email = userModel.Email;
                //await _userManager.AddToRoleAsync(user, userModel.Role);
                await _userManager.UpdateAsync(user);
                //await _roleManager.UpdateAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
