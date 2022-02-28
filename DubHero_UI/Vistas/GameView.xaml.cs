using DubHero_UI.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
namespace DubHero_UI.Vistas
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class GameView : Page
    {
        #region Local Properties
        MediaPlayer _mediaPlayer = new MediaPlayer();
        Stopwatch _stopwatch = new Stopwatch();
        PlaybackManager _playback;

        long _timeToFall = 3000L;
        long _failOffset = 300L;
        long _tooSoonOffset = 1000L;//TODO Adjust these values
        long _currentTime = 0L;
        Thread _playerThread;

        LinkedList<GameNote>[] _tracks;
        #endregion

        #region UWP Related
        public GameView()
        {
            _tracks = new LinkedList<GameNote>[5];
            for (var i = 0; i < _tracks.Length; i++)
            {
                _tracks[i] = new LinkedList<GameNote>();
            }
            _playerThread = new Thread(SongUpdate);
            this.InitializeComponent();
        }

        /// <summary>
        /// OnNavigatedTo parameter must be the name of the map's directory. The midi file must be named
        /// "midimap.midi" and the song file "song.mp3" for it to work.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                var songName = (string)e.Parameter;
                _playback = new PlaybackManager(songName + "/midimap.midi");
                _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(songName + "/song.mp3"));
            }
            base.OnNavigatedTo(e);
        }
        #endregion

        #region Flow control
        public void AnimateNote(GameNote note)
        {
            //TODO haser bomnito esto
            throw new NotImplementedException();
        }

        public void StartSong()
        {
            _mediaPlayer.Play();
            _playback.StartReading();
            _stopwatch.Reset();
            _stopwatch.Start();
            _playerThread.Start();
        }

        [Obsolete]
        //Como el hilo no accede a nada de valor no es necesario usar una
        //implementación más actual.
        public void PauseSong()
        {
            _mediaPlayer.Pause();
            _playback.PauseReading();
            _stopwatch.Stop();
            _playerThread.Suspend();
        }

        [Obsolete]
        //Como el hilo no accede a nada de valor no es necesario usar una
        //implementación más actual.
        public void ResumeSong()
        {
            _mediaPlayer.Play();
            _playback.ResumeReading();
            _stopwatch.Start();
            _playerThread.Resume();
        }

        void SongUpdate()
        {
            while (true)
            {
                _currentTime = _stopwatch.ElapsedMilliseconds;
                CheckNotesToDestroy();
            }
        }

        private void CheckNotesToDestroy()
        {
            foreach (var track in _tracks)
            {
                var note = track.First();
                if (note.MillisSinceRead >= _timeToFall + _failOffset)
                {
                    //Destroy note in the view
                    track.Remove(note);
                }
            }
        }
        void CheckPlayedNote(int trackIndex)
        {
            var targetTrack = _tracks[trackIndex];
            var nextNote = targetTrack.First.Value;
            if (nextNote != null)
            {
                var differenceToPerfect = Math.Abs(nextNote.MillisSinceRead - _timeToFall);
                var isOnTime = differenceToPerfect <= _failOffset;//0 is perfect
                if (isOnTime)
                {
                    targetTrack.RemoveFirst();
                    //TODO Destroy the note (Correct note animation)
                    //TODO Manage correct note
                }
                else if (differenceToPerfect <= _tooSoonOffset)
                {
                    targetTrack.RemoveFirst();
                    //TODO Destroy note (Failed note animation)
                    //TODO manage failed note
                }
            }
        }
        #endregion

        #region User Input
        public void MainPage_KeyDown(object sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.W)
                CheckPlayedNote(0);

            if (args.VirtualKey == VirtualKey.D)
                CheckPlayedNote(1);

            if (args.VirtualKey == VirtualKey.J)
                CheckPlayedNote(2);

            if (args.VirtualKey == VirtualKey.I)
                CheckPlayedNote(3);

            if (args.VirtualKey == VirtualKey.O)
                CheckPlayedNote(4);
        }
        #endregion
    }
}
