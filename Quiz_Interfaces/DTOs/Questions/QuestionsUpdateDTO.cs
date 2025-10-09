using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Quiz_Interfaces.DTOs.Choices;
using Quiz_Interfaces.DTOs.Class;
using Quiz_Interfaces.DTOs.Subject;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.DTOs.Questions
{
    public class QuestionsUpdateDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }// e.g. "q_20250925_001"
        public string SubjectId { get; set; }  // "Math", "Physics", "History", ...
        public string ClassId { get; set; } // e.g. "Grade 10", "Grade 11"
        [BsonRepresentation(BsonType.String)]
        public string Difficulty { get; set; } // "Easy", "Medium", "Hard"
        public string QuestionText { get; set; }
        public List<ChoiceUpdateDTO> Choices { get; set; } = new();
        public string Explanation { get; set; }
        public List<string> Tags { get; set; } = new(); // e.g. ["algebra", "grade10"]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
