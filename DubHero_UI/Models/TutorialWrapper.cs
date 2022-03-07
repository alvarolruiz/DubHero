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

        private SongView songView;
        private MediaTimelineController _mediaTimelineController;

        public TutorialWrapper(SongView songView, MediaTimelineController mediaTimelineController)
        {
            this.SongView = songView;
            this._mediaTimelineController = mediaTimelineController;
        }

        public MediaTimelineController MediaTimelineController { get => _mediaTimelineController; set => _mediaTimelineController = value; }
        public SongView SongView { get => songView; set => songView=value; }
    }
}
