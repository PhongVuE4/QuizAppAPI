using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz_Interfaces.DTOs.Class;
using Quiz_Interfaces.IRepository;
using Quiz_Interfaces.Models;

namespace QuizAPI.Controllers
{
    [Route("api/class")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassRepository _classRepository;
        public ClassesController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllClasses()
        {
            var classes = await _classRepository.GetAllClassesAsync();
            return StatusCode(classes.Code, classes);
        }
        [HttpPost("create-class")]
        public async Task<IActionResult> CreateClass(ClassCreateDTO create)
        {
            var createClass = await _classRepository.CreateClassAsync(create);
            return StatusCode(createClass.Code, createClass);
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateClass(ClassUpdateDTO classUpdate){
            var updateClass = await _classRepository.UpdateClassAsync(classUpdate);
            return StatusCode(updateClass.Code, updateClass);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteClass(string id){
            var deleteClass = await _classRepository.DeleteClassAsync(id);
            return StatusCode(deleteClass.Code, deleteClass);
        }
    }
}
