using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.DTOs.Subject
{
    public class SubjectReponseDTO
    {
        public string SubjectId { get; set; }
        public string SubjectName { get; set; } // e.g. "Math", "Physics", "History"
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
