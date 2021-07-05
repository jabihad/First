using AutoMapper;
using Entities.Models;
using FileUploadApi.Model;
using FileUploadApi.Repositories;
using FileUploadApi.Services.Admin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Admin.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<Extension> _extension;
        private readonly IRepository<File> _file;
        private readonly IMapper _mapper;
        public AdminService(IRepository<Extension> extension, IRepository<File> file, IMapper mapper)
        {
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
        public async Task<bool>CreateExtension(ExtensionModel extensionModel)
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
    }
}
