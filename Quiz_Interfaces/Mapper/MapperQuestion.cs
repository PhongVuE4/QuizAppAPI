using AutoMapper;
using Quiz_Interfaces.DTOs.Choices;
using Quiz_Interfaces.DTOs.Questions;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.Mapper
{
    public class MapperQuestion : Profile
    {
        public MapperQuestion()
        {
            CreateMap<QuestionsCreateDTO, Question>().ForMember(dest => dest.Id, opt => opt.Ignore())
                                                  .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                                                  .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<ChoiceCreateDTO, Choice>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
