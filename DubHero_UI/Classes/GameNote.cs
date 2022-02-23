using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubHero_UI.Classes
{
    public class GameNote
    {
        public int NoteNumber { get; }
        public long MillisSinceRead { get; set; }

        public GameNote(int noteNumber)
        {
            this.NoteNumber = noteNumber;
        }
    }
}
