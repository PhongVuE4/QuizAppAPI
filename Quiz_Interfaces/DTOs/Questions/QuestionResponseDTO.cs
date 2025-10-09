using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Quiz_Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.DTOs.Questions
{
    public class QuestionResponseDTO
    {
        public string QuestionId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Subject { get; set; }    // e.g. "Math"
        [BsonRepresentation(BsonType.ObjectId)]
        public string Class { get; set; }      // e.g. "Grade 10"
        public string Difficulty { get; set; } // "Easy", "Medium", "Hard"
        public string QuestionText { get; set; }
        public List<Choice> Choices { get; set; } = new();
        public string Explanation { get; set; }
        public List<string> Tags { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
