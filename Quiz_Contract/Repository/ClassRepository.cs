using AutoMapper;
using MongoDB.Driver;
using Quiz_Common.Results;
using Quiz_Interfaces.DTOs.Class;
using Quiz_Interfaces.IRepository;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Infrastructure.Repository
{
    public class ClassRepository : IClassRepository
    {
        private readonly IMongoCollection<Class> _classes;
        private readonly IMapper _mapper;
        public ClassRepository(MongoDBContext context, IMapper mapper)
        {
            _classes = context.Classes;
            _mapper = mapper;
        }
        public async Task<ServiceResult<List<Class>>> GetAllClassesAsync()
        {
            var classes = await _classes.Find(_ => true).ToListAsync();
            if(classes == null || !classes.Any())
            {
                return ServiceResult<List<Class>>.Success(classes, "Danh sách lớp trống", code: 404);
            }
            return ServiceResult<List<Class>>.Success(classes, "Lấy danh sách lớp thành công", code: 200);
        }
        public async Task<ServiceResult<Class>> CreateClassAsync(ClassCreateDTO createClass)
        {
            if (createClass == null)
                return ServiceResult<Class>.Failure("Không liệu rỗng", code: 400);
            if (string.IsNullOrEmpty(createClass.Classlevel))
                return ServiceResult<Class>.Failure("Tên lớp không được rỗng", code: 400);
            createClass.Classlevel = createClass.Classlevel.Trim();
            var existed = await _classes.Find(a => a.Classlevel.ToLower() == createClass.Classlevel.ToLower()).FirstOrDefaultAsync();
            if (existed != null)
                return ServiceResult<Class>.Failure("Lớp đã tồn tại", code: 400);
            var newClass =  _mapper.Map<Class>(createClass);

            try
            {
                await _classes.InsertOneAsync(newClass);
                return ServiceResult<Class>.Success(newClass, "Tạo lớp thành công.", code: 200);
            }
            catch (MongoWriteException ex)
            {
                return ServiceResult<Class>.Failure("Lỗi ghi dữ liệu vào database: " + ex.Message, code: 500);
            }
            catch (Exception ex)
            {
                return ServiceResult<Class>.Failure("Lỗi không xác định: " + ex.Message, code: 500);
            }
        }
        public async Task<ServiceResult<ClassUpdateDTO>> UpdateClassAsync(ClassUpdateDTO updateClass)
        {
            if (updateClass == null)
                return ServiceResult<ClassUpdateDTO>.Failure("Dữ liệu rỗng", code: 400);
            if (string.IsNullOrWhiteSpace(updateClass.ClassId) || updateClass.ClassId.Length != 24)
                return ServiceResult<ClassUpdateDTO>.Failure("Id không hợp lệ", code: 400);
            if (string.IsNullOrWhiteSpace(updateClass.Classlevel))
                return ServiceResult<ClassUpdateDTO>.Failure("Tên lớp không được rỗng", code: 400);
            updateClass.Classlevel = updateClass.Classlevel.Trim();
            var existingClass = await _classes.Find(c => c.Classlevel.ToLower() == updateClass.Classlevel.ToLower() && c.ClassId != updateClass.ClassId).FirstOrDefaultAsync();
            if (existingClass != null)
                return ServiceResult<ClassUpdateDTO>.Failure("Lớp đã tồn tại", code: 400);
            var classToUpdate = await _classes.Find(c => c.ClassId == updateClass.ClassId).FirstOrDefaultAsync();
            if (classToUpdate == null)
                return ServiceResult<ClassUpdateDTO>.Failure("Không tìm thấy lớp với Id đã cho", code: 404);
            classToUpdate.Classlevel = updateClass.Classlevel;
            try
            {
                var result = await _classes.ReplaceOneAsync(c => c.ClassId == updateClass.ClassId, classToUpdate);
                if (result.ModifiedCount == 0)
                    return ServiceResult<ClassUpdateDTO>.Failure("Cập nhật lớp không thành công", code: 500);
                return ServiceResult<ClassUpdateDTO>.Success(updateClass, "Cập nhật lớp thành công", code: 200);
            }
            catch (MongoWriteException ex)
            {
                return ServiceResult<ClassUpdateDTO>.Failure("Lỗi ghi dữ liệu vào database: " + ex.Message, code: 500);
            }
            catch (Exception ex)
            {
                return ServiceResult<ClassUpdateDTO>.Failure("Lỗi không xác định: " + ex.Message, code: 500);
            }
        }
        public async Task<ServiceResult<Class>> DeleteClassAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || id.Length != 24)
                return ServiceResult<Class>.Failure("Id không hợp lệ", code: 400);
            var classToDelete = await _classes.Find(c => c.ClassId == id && c.IsActive).FirstOrDefaultAsync();
            if (classToDelete == null)
                return ServiceResult<Class>.Failure("Không tìm thấy lớp với Id đã cho", code: 404);
            try
            {
                var update = Builders<Class>.Update.Set(c => c.IsActive, false);
                var result = await _classes.UpdateOneAsync(c => c.ClassId == id, update);
                if (result.ModifiedCount == 0)
                    return ServiceResult<Class>.Failure("Xóa lớp không thành công", code: 500);
                return ServiceResult<Class>.Success(null, "Xóa lớp thành công", code: 200);
            }
            catch (MongoWriteException ex)
            {
                return ServiceResult<Class>.Failure("Lỗi ghi dữ liệu vào database: " + ex.Message, code: 500);
            }
            catch (Exception ex)
            {
                return ServiceResult<Class>.Failure("Lỗi không xác định: " + ex.Message, code: 500);
            }
        }
    }
}
