using AutoMapper;
using Quiz_Common;
using Quiz_Interfaces.DTOs.Choices;
using Quiz_Interfaces.DTOs.Questions;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Quiz_Interfaces.Mapper
{
    public class MapperQuestion : Profile
    {
        public MapperQuestion()
        {
            CreateMap<QuestionResponseDTO, QuestionResponseDTO>().ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => TimeHelper.ConvertToVietnamTime(src.CreatedAt)))
                                                     .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => TimeHelper.ConvertToVietnamTime(src.UpdatedAt)));

            CreateMap<QuestionsCreateDTO, Question>().ForMember(dest => dest.QuestionId, opt => opt.Ignore())
                                                  .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => TimeHelper.GetVietnamCurrentTime()))
                                                  .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => TimeHelper.GetVietnamCurrentTime()));

            CreateMap<ChoiceCreateDTO, Choice>();
            
            CreateMap<ChoiceUpdateDTO, Choice>();

            CreateMap<QuestionsUpdateDTO, Question>().ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.Id))
                                                  .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                                                  .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => TimeHelper.GetVietnamCurrentTime()));
        }
    }
}
