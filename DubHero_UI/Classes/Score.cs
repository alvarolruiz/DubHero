using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DubHero_UI.Classes
{
    public class Score
    {
        public int Points { get; set; }
        public int CorrectValue { get; set; }
        public TextBlock ScoreText { get; set; }
        
        public void Correct()
        {
            Points += CorrectValue;
            ScoreText.Text = Points.ToString();
        }
    }
}
