using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace DubHero_UI.Classes
{
    public class GameNote
    {
        public int NoteNumber { get; }
        public long ReadTime { get; set; }
        public Ellipse Shape { get; set; }
        public Canvas Track { get; set; }

        public int TrackIndex {
            get
            {
                int index = -1;
                switch (NoteNumber)
                {
                    case 60: index = 0; break;
                    case 62: index = 1; break;
                    case 64: index = 2; break;
                    case 65: index = 3; break;
                    case 67: index = 4; break;
                }
                return index;
            }
        }

        public GameNote(int noteNumber)
        {
            this.NoteNumber = noteNumber;
            ReadTime = 0L;
        }

        public void DeleteFromView()
        {
            if (Shape != null && Track != null)
            {
                var dispatcher = Shape.Dispatcher;
                dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Track.Children.Remove(Shape);
                });
            }
        }
    }
}
