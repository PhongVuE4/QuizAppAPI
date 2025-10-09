using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz_Interfaces.DTOs.Questions;
using Quiz_Interfaces.IRepository;
using Quiz_Interfaces.Models;

namespace QuizAPI.Controllers
{
    [Route("api/question")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        public QuestionsController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _questionRepository.GetAllQuestionsAsync();
            if (!questions.IsSuccess)
                return BadRequest(new { error = questions.Message });
            return Ok(questions.Data);
        }
        [HttpGet("questionId")]
        public async Task<IActionResult> GetQuestionId(string id)
        {
            var questions = await _questionRepository.GetQuestionByIdAsync(id);
            return StatusCode(questions.Code, questions);
        }
        [HttpGet("subjectName")]
        public async Task<IActionResult> GetQuestionsBySubject(string subjectName)
        {
            var result = await _questionRepository.GetQuestionsBySubjectAsync(subjectName);
            return StatusCode(result.Code, result);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateQuestions(QuestionsCreateDTO question)
        {
            var result = await _questionRepository.CreateQuestionAsync(question);
            return StatusCode(result.Code, result);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuestions(QuestionsUpdateDTO question)
        {
            var result = await _questionRepository.UpdateQuestionAsync(question);
            return StatusCode(result.Code, result);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteQuestions(string questionId)
        {
            var result = await _questionRepository.DeleteQuestionAsync(questionId);
            return StatusCode(result.Code, result);
        }
        
    }
}
