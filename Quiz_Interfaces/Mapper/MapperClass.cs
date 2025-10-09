using AutoMapper;
using Quiz_Interfaces.DTOs.Class;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.Mapper
{
    public class MapperClass : Profile
    {
        public MapperClass()
        {
            CreateMap<ClassCreateDTO, Class>().ForMember(dest => dest.ClassId, opt => opt.Ignore())
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore()).ForMember(dest => dest.UpdateAt, opt => opt.Ignore());
        }
    }
}
