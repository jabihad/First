using AutoMapper;
using Entities.DTO;
using Entities.Models;
using FileUploadApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<LoginActivityModel, LoginActivity>();
            CreateMap<ExtensionModel, Extension>().ReverseMap();
            CreateMap<FileModel, File>().ReverseMap();
            CreateMap<CategoryModel, Category>().ReverseMap();
        }
    }
}
