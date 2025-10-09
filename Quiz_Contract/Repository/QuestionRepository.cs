using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Quiz_Common;
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
        private readonly IMongoCollection<Subject> _subjects;
        public QuestionRepository(MongoDBContext context, IMapper mapper)
        {
            _questions = context.Questions;
            _mapper = mapper;
            _subjects = context.Subjects;
        }
        public async Task<ServiceResult<List<QuestionResponseDTO>>> GetAllQuestionsAsync()
        {
            var pipeline = _questions.Aggregate().Match(a=> a.IsActive)
                                                .Lookup("subjects", "SubjectId", "_id", "SubjectData")
                                                .Lookup("classes", "ClassId", "_id", "ClassData")
                                                .Project(new BsonDocument
                                                {
                                                    { "_id", 0 },
                                                    { "QuestionId", new BsonDocument("$toString", "$_id")},// map _id -> QuestionId (chuỗi)
                                                    { "QuestionText", 1 },
                                                    { "Difficulty", 1 },
                                                    { "Choices", 1 },
                                                    { "Explanation", 1 },
                                                    { "Tags", 1 },
                                                    { "Subject", new BsonDocument("$arrayElemAt", new BsonArray { "$SubjectData.SubjectName", 0 }) },
                                                    { "Class", new BsonDocument("$arrayElemAt", new BsonArray { "$ClassData.Classlevel", 0 }) },
                                                    { "CreateAt", 1},
                                                    { "UpdatedAt", 1  }
                                                });
            var results = await pipeline.ToListAsync();
            if (results == null || !results.Any())
            {
                return ServiceResult<List<QuestionResponseDTO>>.Failure("Không có câu hỏi nào", code: 404);
            }
            var rawList = results.Select(a => BsonSerializer.Deserialize<QuestionResponseDTO>(a)).ToList();
            var questionReponses = _mapper.Map<List<QuestionResponseDTO>>(rawList);
            return ServiceResult<List<QuestionResponseDTO>>.Success(questionReponses, "Thành công", code: 200);
        }
        public async Task<ServiceResult<QuestionResponseDTO>> GetQuestionByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return ServiceResult<QuestionResponseDTO>.Failure("Id null hoặc rỗng", code: 400);
            
            if (id.Length != 24)
                return ServiceResult<QuestionResponseDTO>.Failure("Id không hợp lệ", code: 400);
            var pipeline = _questions.Aggregate().Match(a => a.QuestionId == id).Match(a => a.IsActive == true)
                                                .Lookup("subjects", "SubjectId", "_id", "SubjectData")
                                                .Lookup("classes", "ClassId", "_id", "ClassData")
                                                .Project(new BsonDocument
                                                {
                                                    { "_id", 0 },
                                                    { "QuestionId", new BsonDocument("$toString", "$_id")},// map _id -> QuestionId (chuỗi)
                                                    { "QuestionText", 1 },
                                                    { "Difficulty", 1 },
                                                    { "Choices", 1 },
                                                    { "Explanation", 1 },
                                                    { "Tags", 1 },
                                                    { "Subject", new BsonDocument("$arrayElemAt", new BsonArray { "$SubjectData.SubjectName", 0 }) },
                                                    { "Class", new BsonDocument("$arrayElemAt", new BsonArray { "$ClassData.Classlevel", 0 }) },
                                                    { "CreateAt", 1},
                                                    { "UpdatedAt", 1  }
                                                });
            var result = await pipeline.FirstOrDefaultAsync();
            if (result == null)
            {
                return ServiceResult<QuestionResponseDTO>.Failure("Không tìm thấy câu hỏi", code: 404);
            }
            var questionReponse = BsonSerializer.Deserialize<QuestionResponseDTO>(result);
            return ServiceResult<QuestionResponseDTO>.Success(questionReponse,"Thành công", code: 200);
        }
        public async Task<ServiceResult<Question>> CreateQuestionAsync(QuestionsCreateDTO dto)
        {
            if(dto == null)
            {
                return ServiceResult<Question>.Failure("Dữ liệu null", code: 400);
            }
            if(string.IsNullOrWhiteSpace(dto.QuestionText))
            {
                return ServiceResult<Question>.Failure("QuestionText không được để trống.", code: 400);
            }
            if(dto.Choices == null || !dto.Choices.Any())
            {
                return ServiceResult<Question>.Failure("Choices không được để trống.", code: 400);
            }
            if(dto.Choices.Count(c => c.IsCorrect) == 0)
            {
                return ServiceResult<Question>.Failure("Phải có ít nhất một đáp án đúng.", code: 400);
            }
            if ((dto.Choices.Count(a => a.IsCorrect) < 1))
            {
                return ServiceResult<Question>.Failure("Chỉ được có một đáp án đúng.");
            }
            if(dto.Choices.Count() < 2)
            {
                return ServiceResult<Question>.Failure("Mỗi câu hỏi phải có ít nhất hai lựa chọn.", code: 400);
            }
            var question = _mapper.Map<Question>(dto);
            try
            {
                await _questions.InsertOneAsync(question);
                return ServiceResult<Question>.Success(question, "Tạo câu hỏi thành công", code: 200);
            }
            catch (MongoWriteException ex)
            {
                // lỗi ghi DB (ví dụ trùng key, schema sai, v.v.)
                return ServiceResult<Question>.Failure("Database write error: " + ex.Message, code: 503);
            }
            catch (Exception ex)
            {

                return ServiceResult<Question>.Failure("Lỗi không xác định: " + ex.Message, code: 500);
            }
            
        }
        //public async Task<int> CreateManyQuestionAsync(List<Question> questions)
        //{

        //}
        public async Task<ServiceResult<QuestionsUpdateDTO>> UpdateQuestionAsync(QuestionsUpdateDTO questionDTO)
        {
            if (questionDTO == null || string.IsNullOrEmpty(questionDTO.Id))
            {
                return ServiceResult<QuestionsUpdateDTO>.Failure("Dữ liệu hoặc Id null", code: 400);
            }
            if(questionDTO.Id.Length != 24)
            {
                return ServiceResult<QuestionsUpdateDTO>.Failure("Id không hợp lệ", code: 400);
            }
            if(questionDTO.SubjectId != null && string.IsNullOrWhiteSpace(questionDTO.SubjectId))
            {
                return ServiceResult<QuestionsUpdateDTO>.Failure("Môn học không được để trống.", code: 400);
            }
            if (questionDTO.Difficulty != null && string.IsNullOrEmpty(questionDTO.Difficulty))
                return ServiceResult<QuestionsUpdateDTO>.Failure("Độ khó không được để trống.", code: 400);
            if (questionDTO.QuestionText != null && string.IsNullOrWhiteSpace(questionDTO.QuestionText))
            {
                return ServiceResult<QuestionsUpdateDTO>.Failure("Câu hỏi không được để trống.", code: 400);
            }
            if(questionDTO.Choices != null)
            {
                if(questionDTO.Choices.Count(a => a.IsCorrect) > 1)
                {
                    return ServiceResult<QuestionsUpdateDTO>.Failure("Chỉ được có một đáp án đúng.", code: 400);
                }
                if (questionDTO.Choices == null || !questionDTO.Choices.Any())
                {
                    return ServiceResult<QuestionsUpdateDTO>.Failure("Lựa chọn không được để trống.", code: 400);
                }
                if (questionDTO.Choices.Count() < 2)
                {
                    return ServiceResult<QuestionsUpdateDTO>.Failure("Mỗi câu hỏi phải có ít nhất hai lựa chọn.", code: 400);
                }
                if (questionDTO.Choices.Count(c => c.IsCorrect) == 0)
                {
                    return ServiceResult<QuestionsUpdateDTO>.Failure("Phải có một đáp án đúng.", code: 400);
                }
            }
            var existingQuestion = await _questions.Find(q => q.QuestionId == questionDTO.Id && q.IsActive).FirstOrDefaultAsync();
            if(existingQuestion == null)
                return ServiceResult<QuestionsUpdateDTO>.Failure("Câu hỏi không tồn tại", code: 404);

            List<Choice> mappedChoices = existingQuestion.Choices;
            if (questionDTO.Choices != null)
                mappedChoices = _mapper.Map<List<Choice>>(questionDTO.Choices);

            var update = Builders<Question>.Update
                .Set(q => q.QuestionText, questionDTO.QuestionText ?? existingQuestion.QuestionText)
                .Set(q => q.Difficulty, questionDTO.Difficulty ?? existingQuestion.Difficulty)
                .Set(q => q.SubjectId, questionDTO.SubjectId ?? existingQuestion.SubjectId)
                .Set(q => q.ClassId, questionDTO.ClassId ?? existingQuestion.ClassId)
                .Set(q => q.Choices, mappedChoices)
                .Set(q => q.Explanation, questionDTO.Explanation ?? existingQuestion.Explanation)
                .Set(q => q.Tags, questionDTO.Tags ?? existingQuestion.Tags)
                .Set(q => q.UpdatedAt, TimeHelper.GetVietnamCurrentTime());

            var filter = Builders<Question>.Filter.Where(q => q.QuestionId == questionDTO.Id && q.IsActive);
            var result = await _questions.UpdateOneAsync(filter, update);
            if (result.ModifiedCount == 0)
                return ServiceResult<QuestionsUpdateDTO>.Failure("Không có thay đổi nào được thực hiện.", code: 304);

            return ServiceResult<QuestionsUpdateDTO>.Success(questionDTO, "Cập nhật câu hỏi thành công", code: 200);
        }
        public async Task<ServiceResult<Question>> DeleteQuestionAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return ServiceResult<Question>.Failure("Id null hoặc rỗng", code: 400);
            if (id.Length != 24)
                return ServiceResult<Question>.Failure("Id không hợp lệ", code: 400);
            var question = await _questions.Find(q => q.QuestionId == id && q.IsActive == true).FirstOrDefaultAsync();
            if(question == null)
            {
                return ServiceResult<Question>.Failure("Câu hỏi không tồn tại", code: 404);
            }
            var update = Builders<Question>.Update.Set(q => q.IsActive, false).Set(q => q.UpdatedAt, TimeHelper.GetVietnamCurrentTime());
            var result = await _questions.UpdateOneAsync(q => q.QuestionId == id, update);
            if (result.ModifiedCount == 0)
                return ServiceResult<Question>.Failure("Xóa câu hỏi thất bại", code: 500);
            return ServiceResult<Question>.Success(null, "Xóa câu hỏi thành công", code: 200);
        }
        public async Task<ServiceResult<List<QuestionResponseDTO>>> GetQuestionsBySubjectAsync(string subjectName)
        {
            if(string.IsNullOrWhiteSpace(subjectName))
                return ServiceResult<List<QuestionResponseDTO>>.Failure("Môn học không được để rỗng", code: 400);
            var subject = await _subjects.Find(s => s.SubjectName.ToLower() == subjectName.ToLower()).FirstOrDefaultAsync();
            if(subject == null)
                return ServiceResult<List<QuestionResponseDTO>>.Failure("Không tìm thấy môn học này", code: 404);

            var pipeline = _questions.Aggregate().Match(a => a.SubjectId == subject.SubjectId.ToString()).Match(a => a.IsActive)
                                                .Lookup("subjects", "SubjectId", "_id", "SubjectData")
                                                .Lookup("classes", "ClassId", "_id", "ClassData")
                                                .Project(new BsonDocument
                                                {
                                                    { "_id", 0 },
                                                    { "QuestionId", new BsonDocument("$toString", "$_id")},// map _id -> QuestionId (chuỗi)
                                                    { "QuestionText", 1 },
                                                    { "Difficulty", 1 },
                                                    { "Choices", 1 },
                                                    { "Explanation", 1 },
                                                    { "Tags", 1 },
                                                    { "Subject", new BsonDocument("$arrayElemAt", new BsonArray { "$SubjectData.SubjectName", 0 }) },
                                                    { "Class", new BsonDocument("$arrayElemAt", new BsonArray { "$ClassData.Classlevel", 0 }) }
                                                });
            var results = await pipeline.ToListAsync();
            if (results == null || !results.Any())
            {
                return ServiceResult<List<QuestionResponseDTO>>.Failure("Không có câu hỏi nào", code: 404);
            }
            var questionReponses = results.Select(a => BsonSerializer.Deserialize<QuestionResponseDTO>(a)).ToList();
            return ServiceResult<List<QuestionResponseDTO>>.Success(questionReponses, "Thành công", code: 200);
        }
    }
}
