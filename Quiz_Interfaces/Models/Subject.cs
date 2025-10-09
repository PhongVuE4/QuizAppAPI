using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.Models
{
    public class Subject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SubjectId { get; set; }
        public string SubjectName { get; set; } // e.g. "Math", "Physics", "History"
        public bool IsActive { get; set; } = true; // Soft delete flag
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
