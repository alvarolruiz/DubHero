using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubHero_UI.Classes
{
    public class Score
    {
        public int Points { get; set; }
        public int CorrectValue { get; set; }
        
        public void Correct()
        {
            Points += CorrectValue;
        }
    }
}
