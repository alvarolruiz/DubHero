using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media;

namespace DubHero_UI.Models
{
    public class TutorialWrapper
    {

        private String songName;
        private MediaTimelineController _mediaTimelineController;

        public TutorialWrapper(string songName, MediaTimelineController mediaTimelineController)
        {
            this.songName = songName;
            this._mediaTimelineController = mediaTimelineController;
        }

        public string SongName { get => songName; set => songName = value; }
        public MediaTimelineController MediaTimelineController { get => _mediaTimelineController; set => _mediaTimelineController = value; }
    }
}
