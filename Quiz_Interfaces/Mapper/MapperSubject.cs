using AutoMapper;
using Quiz_Common;
using Quiz_Interfaces.DTOs.Subject;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.Mapper
{
    public class MapperSubject : Profile
    {
        public MapperSubject()
        {
            CreateMap<Subject, SubjectReponseDTO>().ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeHelper.ConvertToVietnamTime(src.CreateAt)))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeHelper.ConvertToVietnamTime(src.UpdateAt)));
            CreateMap<SubjectCreateDTO,Subject>().ForMember(dest => dest.SubjectId, opt => opt.Ignore())
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => TimeHelper.GetVietnamCurrentTime()))
                .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeHelper.GetVietnamCurrentTime()));

            CreateMap<SubjectUpdateDTO, Subject>().ForMember(dest => dest.SubjectId, opt => opt.MapFrom(src => src.SubjectId))
                                                    .ForMember(dest => dest.CreateAt, opt => opt.Ignore())
                                                    .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => TimeHelper.GetVietnamCurrentTime()));
        }
    }
}
