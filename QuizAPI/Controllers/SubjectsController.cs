using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz_Interfaces.DTOs.Subject;
using Quiz_Interfaces.IRepository;

namespace QuizAPI.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectsController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }
        [HttpGet("subjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var subjects = await _subjectRepository.GetAllSubjectsAsync();
            return StatusCode(subjects.Code, subjects);
        }
        [HttpGet("subjectId")]
        public async Task<IActionResult> GetSubjectById(string id)
        {
            var subject = await _subjectRepository.GetSubjectByIdAsync(id);
            return StatusCode(subject.Code, subject);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateSubject(SubjectCreateDTO subject)
        {
            var result = await _subjectRepository.CreateSubjectAsync(subject);
            return StatusCode(result.Code, result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSubject(SubjectUpdateDTO subject)
        {
            var result = await _subjectRepository.UpdateSubjectAsync(subject);
            return StatusCode(result.Code, result);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            var result = await _subjectRepository.DeleteSubjectAsync(id);
            return StatusCode(result.Code, result);
        }
    }
}
