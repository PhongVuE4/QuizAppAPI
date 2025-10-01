using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.Models
{
    public class Choice
    {
        public string Id { get; set; }    // e.g. "A", "B", "C", "D"
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
