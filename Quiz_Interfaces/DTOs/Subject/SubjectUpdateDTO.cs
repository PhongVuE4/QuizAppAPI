using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.DTOs.Subject
{
    public class SubjectUpdateDTO
    {
        public string SubjectId { get; set; } // e.g. "64f0c9e2b4d1c2a5f6e7d8c9"
        public string SubjectName { get; set; } // e.g. "Math", "Physics", "History"
    }
}
