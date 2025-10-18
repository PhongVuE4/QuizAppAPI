using Quiz_Common.Results;
using Quiz_Interfaces.DTOs.Questions;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.IRepository
{
    public interface IQuestionRepository
    {
        Task<ServiceResult<List<QuestionResponseDTO>>> GetAllQuestionsAsync();
        Task<ServiceResult<QuestionResponseDTO>> GetQuestionByIdAsync(string id);
        Task<ServiceResult<List<QuestionResponseDTO>>> GetQuestionsByClassandSubjectAsync(string? classLevel, string? subject);
        Task<ServiceResult<Question>> CreateQuestionAsync(QuestionsCreateDTO dto);
        //Task<int> CreateManyQuestionAsync(List<Models.Question> questions);
        Task<ServiceResult<QuestionsUpdateDTO>> UpdateQuestionAsync(QuestionsUpdateDTO question);
        Task<ServiceResult<Question>> DeleteQuestionAsync(string id);
        Task<ServiceResult<List<QuestionResponseDTO >>> GetQuestionsBySubjectAsync(string subject);
    }
}
