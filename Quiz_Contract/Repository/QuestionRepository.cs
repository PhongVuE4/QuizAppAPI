using AutoMapper;
using MongoDB.Driver;
using Quiz_Common.Results;
using Quiz_Interfaces.DTOs.Questions;
using Quiz_Interfaces.IRepository;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Infrastructure.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IMongoCollection<Question> _questions;
        private readonly IMapper _mapper;
        public QuestionRepository(MongoDBContext context, IMapper mapper)
        {
            _questions = context.Questions;
            _mapper = mapper;
        }
        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            return await _questions.Find(q => true).ToListAsync();
        }
        public async Task<Question> GetQuestionByIdAsync(string id)
        {
            return await _questions.Find(q => q.Id == id).FirstOrDefaultAsync();
        }
        public async Task<ServiceResult<Question>> CreateQuestionAsync(QuestionsCreateDTO dto)
        {
            if(dto == null)
            {
                return ServiceResult<Question>.Failure("Dữ liệu null");
            }
            if(string.IsNullOrWhiteSpace(dto.QuestionText))
            {
                return ServiceResult<Question>.Failure("QuestionText không được để trống.");
            }
            if(dto.Choices == null || !dto.Choices.Any())
            {
                return ServiceResult<Question>.Failure("Choices không được để trống.");
            }
            if(dto.Choices.Count(c => c.IsCorrect) == 0)
            {
                return ServiceResult<Question>.Failure("Phải có ít nhất một đáp án đúng.");
            }
            var question = _mapper.Map<Question>(dto);
            try
            {
                await _questions.InsertOneAsync(question);
                return ServiceResult<Question>.Success(question, "Tạo câu hỏi thành công");
            }
            catch (MongoWriteException ex)
            {
                // lỗi ghi DB (ví dụ trùng key, schema sai, v.v.)
                return ServiceResult<Question>.Failure("Database write error: " + ex.Message);
            }
            catch (Exception ex)
            {

                return ServiceResult<Question>.Failure("Lỗi không xác định: " + ex.Message);
            }
            
        }
        //public async Task<int> CreateManyQuestionAsync(List<Question> questions)
        //{

        //}
        public async Task UpdateQuestionAsync(Question question)
        {
            question.UpdatedAt = DateTime.UtcNow;
            await _questions.ReplaceOneAsync(q => q.Id == question.Id, question);
        }
        public async Task DeleteQuestionAsync(string id)
        {
            await _questions.DeleteOneAsync(q => q.Id == id);
        }
        public async Task<List<Question>> GetQuestionsByCategoryAsync(string category)
        {
            return await _questions.Find(q => q.Category == category).ToListAsync();
        }

    }
}
