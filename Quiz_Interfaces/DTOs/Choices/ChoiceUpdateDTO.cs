using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Interfaces.DTOs.Choices
{
    public class ChoiceUpdateDTO
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
