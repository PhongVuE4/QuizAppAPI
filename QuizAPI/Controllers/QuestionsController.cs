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
            return Ok(questions);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateQuestions(QuestionsCreateDTO question)
        {
            var result = await _questionRepository.CreateQuestionAsync(question);
            if(!result.IsSuccess)
                return BadRequest(new { error = result.Message });
            return Ok(result.Data);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuestions(Question question)
        {
            await _questionRepository.UpdateQuestionAsync(question);
            return Ok();
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteQuestions(string questionId)
        {
            await _questionRepository.DeleteQuestionAsync(questionId);
            return Ok();
        }
    }
}
