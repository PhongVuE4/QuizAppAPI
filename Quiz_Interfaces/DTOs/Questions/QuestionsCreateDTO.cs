using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Quiz_Interfaces.DTOs.Choices;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.DTOs.Questions
{
    public class QuestionsCreateDTO
    {
        public string Category { get; set; }  // "Math", "Physics", "History", ...
        public string Difficulty { get; set; } // "Easy", "Medium", "Hard"
        public string QuestionText { get; set; }
        public List<ChoiceCreateDTO> Choices { get; set; } = new();
        public string Explanation { get; set; }
        public List<string> Tags { get; set; } = new(); // e.g. ["algebra", "grade10"]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
