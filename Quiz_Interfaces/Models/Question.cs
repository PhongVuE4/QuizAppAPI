using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Quiz_Interfaces.Models
{
    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string QuestionId { get; set; }// e.g. "q_20250925_001"
        [BsonRepresentation(BsonType.ObjectId)]
        public string SubjectId { get; set; }  // "Math", "Physics", "History", ...
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClassId { get; set; } // e.g. "Grade 10", "Grade 11"
        [BsonRepresentation(BsonType.String)]
        public string Difficulty { get; set; } // "Easy", "Medium", "Hard"
        public string QuestionText { get; set; }
        public List<Choice> Choices { get; set; } = new();
        public string Explanation { get; set; }
        public List<string> Tags { get; set; } = new(); // e.g. ["algebra", "grade10"]
        public bool IsActive { get; set; } = true; // Soft delete flag
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
