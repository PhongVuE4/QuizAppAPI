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
        Task<List<Models.Question>> GetAllQuestionsAsync();
        Task<Models.Question> GetQuestionByIdAsync(string id);
        Task<ServiceResult<Question>> CreateQuestionAsync(QuestionsCreateDTO dto);
        //Task<int> CreateManyQuestionAsync(List<Models.Question> questions);
        Task UpdateQuestionAsync(Models.Question question);
        Task DeleteQuestionAsync(string id);
        Task<List<Models.Question>> GetQuestionsByCategoryAsync(string category);
    }
}
