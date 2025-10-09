using Quiz_Common.Results;
using Quiz_Interfaces.DTOs.Class;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.IRepository
{
    public interface IClassRepository
    {
        Task<ServiceResult<List<Class>>> GetAllClassesAsync();
        Task<ServiceResult<Class>> CreateClassAsync(ClassCreateDTO createClass);
        Task<ServiceResult<ClassUpdateDTO>> UpdateClassAsync(ClassUpdateDTO updateClass);
        Task<ServiceResult<Class>> DeleteClassAsync(string id);
    }
}
