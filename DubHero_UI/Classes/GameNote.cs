using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace DubHero_UI.Classes
{
    public class GameNote
    {
        public int NoteNumber { get; }
        public long ReadTime { get; set; }
        public int TrackIndex { get; set; }
        public Shape Shape { get; set; }
        public Canvas Track { get; set; }    

        public GameNote(int noteNumber)
        {
            this.NoteNumber = noteNumber;
            ReadTime = 0L;
        }
    }
}
