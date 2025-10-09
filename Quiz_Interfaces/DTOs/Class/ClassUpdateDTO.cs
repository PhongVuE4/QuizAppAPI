using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.DTOs.Class
{
    public class ClassUpdateDTO
    {
        public string ClassId { get; set; } // e.g. "64b8f0c2e1b2c3d4e5f6a7b8"
        public string Classlevel { get; set; } // e.g. "Grade 10", "Grade 11"
    }
}
