using AutoMapper;
using MongoDB.Driver;
using Quiz_Common;
using Quiz_Common.Results;
using Quiz_Interfaces.DTOs.Subject;
using Quiz_Interfaces.IRepository;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Infrastructure.Repository
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IMongoCollection<Subject> _subjects;
        private readonly IMapper _mapper;
        public SubjectRepository(MongoDBContext context, IMapper mapper)
        {
            _subjects = context.Subjects;
            _mapper = mapper;
        }
        public async Task<ServiceResult<List<SubjectReponseDTO>>> GetAllSubjectsAsync()
        {
            var subjects = await _subjects.Find(_ => true).ToListAsync();
            var result = _mapper.Map<List<SubjectReponseDTO>>(subjects);
            if (result == null || !result.Any())
            {
                return ServiceResult<List<SubjectReponseDTO>>.Success(result, "Danh sách môn học trống", code: 404);
            }
            return ServiceResult<List<SubjectReponseDTO>>.Success(result, "Lấy danh sách môn học thành công", code: 200);
        }
        public async Task<ServiceResult<Subject>> GetSubjectByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return ServiceResult<Subject>.Failure("Id null hoặc rỗng", code: 400);
            if (id.Length != 24)
                return ServiceResult<Subject>.Failure("Id không hợp lệ", code: 400);
            var subject = await _subjects.Find(s => s.SubjectId == id).FirstOrDefaultAsync();
            if (subject == null)
            {
                return ServiceResult<Subject>.Failure("Không tìm thấy id", code: 404);
            }
            return ServiceResult<Subject>.Success(subject, "Thành công", code: 200);
        }
        public async Task<ServiceResult<Subject>> CreateSubjectAsync(SubjectCreateDTO subject)
        {
            if (subject == null)
            {
                return ServiceResult<Subject>.Failure("Dữ liệu null", code: 400);
            }
            if (string.IsNullOrWhiteSpace(subject.SubjectName))
            {
                return ServiceResult<Subject>.Failure("SubjectName không được để trống.", code: 400);
            }
            var existingSubject = await _subjects.Find(s => s.SubjectName.ToLower() == subject.SubjectName.ToLower()).FirstOrDefaultAsync();
            if (existingSubject != null)
            {
                return ServiceResult<Subject>.Failure("Môn học đã tồn tại.", code: 400);
            }
            var subjectDTO = _mapper.Map<Subject>(subject);
            try
            {
                await _subjects.InsertOneAsync(subjectDTO);
                return ServiceResult<Subject>.Success(subjectDTO, "Tạo môn học thành công", code: 200);
            }
            catch (MongoWriteException ex)
            {
                // lỗi ghi DB (ví dụ trùng key, schema sai, v.v.)
                return ServiceResult<Subject>.Failure("Database write error: " + ex.Message, code: 503);
            }
            catch (Exception ex)
            {
                return ServiceResult<Subject>.Failure("Lỗi không xác định: " + ex.Message, code: 500);
            }
        }
        public async Task<ServiceResult<Subject>> UpdateSubjectAsync(SubjectUpdateDTO subject)
        {
            if (subject == null)
            {
                return ServiceResult<Subject>.Failure("Dữ liệu null", code: 400);
            }
            if (string.IsNullOrWhiteSpace(subject.SubjectId) || subject.SubjectId.Length != 24)
            {
                return ServiceResult<Subject>.Failure("Id không hợp lệ", code: 400);
            }
            var existingSubject = await _subjects.Find(s => s.SubjectId == subject.SubjectId).FirstOrDefaultAsync();
            if (existingSubject == null)
            {
                return ServiceResult<Subject>.Failure("Không tìm thấy môn học với Id đã cho.", code: 404);
            }
            if (!string.IsNullOrWhiteSpace(subject.SubjectName))
            {
                var duplicateSubject = await _subjects.Find(s => s.SubjectName.ToLower() == subject.SubjectName.ToLower() && s.SubjectId != subject.SubjectId).FirstOrDefaultAsync();
                if (duplicateSubject != null)
                {
                    return ServiceResult<Subject>.Failure("Môn học đã tồn tại.", code: 400);
                }
            }
            try
            {
                _mapper.Map(subject, existingSubject);
                var updateResult = await _subjects.ReplaceOneAsync(s => s.SubjectId == subject.SubjectId, existingSubject);
                if (updateResult.ModifiedCount == 0)
                {
                    return ServiceResult<Subject>.Failure("Môn học không tồn tại.", code: 404);
                }
                return ServiceResult<Subject>.Success(existingSubject, "Cập nhật môn học thành công", code: 200);
            }
            catch (MongoWriteException ex)
            {
                return ServiceResult<Subject>.Failure("Database write error: " + ex.Message, code: 503);
            }
            catch (Exception ex)
            {
                return ServiceResult<Subject>.Failure("Lỗi không xác định: " + ex.Message, code: 500);
            }
        }
        public async Task<ServiceResult<Subject>> DeleteSubjectAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return ServiceResult<Subject>.Failure("Id null hoặc rỗng", code: 400);
            if (id.Length != 24)
                return ServiceResult<Subject>.Failure("Id không hợp lệ", code: 400);
            var subject = await _subjects.Find(s => s.SubjectId == id && s.IsActive).FirstOrDefaultAsync();
            if (subject == null)
            {
                return ServiceResult<Subject>.Failure("Không tìm thấy id", code: 404);
            }
            try
            {
                var update = Builders<Subject>.Update.Set(s => s.IsActive, false);
                var result = await _subjects.UpdateOneAsync(s => s.SubjectId == id, update);
                if (result.ModifiedCount == 0)
                {
                    return ServiceResult<Subject>.Failure("Xóa môn học không thành công.", code: 500);
                }
                return ServiceResult<Subject>.Success(null, "Xóa môn học thành công", code: 200);
            }
            catch (MongoWriteException ex)
            {
                return ServiceResult<Subject>.Failure("Database write error: " + ex.Message, code: 503);
            }
            catch (Exception ex)
            {
                return ServiceResult<Subject>.Failure("Lỗi không xác định: " + ex.Message, code: 500);
            }
        }
    }
}
