using Quiz_Common.Results;
using Quiz_Interfaces.DTOs.Subject;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.IRepository
{
    public interface ISubjectRepository
    {
        Task<ServiceResult<List<SubjectReponseDTO>>> GetAllSubjectsAsync();
        Task<ServiceResult<Subject>> GetSubjectByIdAsync(string id);
        Task<ServiceResult<Subject>> CreateSubjectAsync(SubjectCreateDTO subject);
        Task<ServiceResult<Subject>> UpdateSubjectAsync(SubjectUpdateDTO subject);
        Task<ServiceResult<Subject>> DeleteSubjectAsync(string id);
    }
}
