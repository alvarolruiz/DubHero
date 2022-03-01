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
    public sealed partial class GameView : Page
    {
        #region Local Properties
        /// <summary>
        /// Manages the media playback session of the song mp3 file.
        /// </summary>
        MediaPlayer _mediaPlayer = new MediaPlayer();

        /// <summary>
        /// When started starts counting elapsed time every system tick.
        /// </summary>
        Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Manages the midi real time sequence session and its data.
        /// </summary>
        PlaybackManager _playback;

        /// <summary>
        /// The time a note takes since appearing to hit the perfect line.
        /// It's also the time difference the song has to be reproduced after the midi.
        /// </summary>
        long _timeToFall = 3000L;

        /// <summary>
        /// The time difference a note can be hit before or after passing the perfect moment.
        /// </summary>
        long _failOffset = 300L;

        /// <summary>
        /// The time difference before passing the perfect moment in wich it starts counting as a failed note.
        /// </summary>
        long _tooSoonOffset = 1000L;//TODO Adjust these values

        /// <summary>
        /// Time passed since the song started in milliseconds.
        /// </summary>
        long _currentTime = 0L;

        /// <summary>
        /// Thread that manages the logic behind all the falling notes.
        /// </summary>
        Thread _playerThread;

        /// <summary>
        /// A list of tracks containing a heap of notes that are already falling.
        /// </summary>
        LinkedList<Classes.GameNote>[] _tracks;
        #endregion

        #region UWP Related
        public GameView()
        {
            _tracks = new LinkedList<Classes.GameNote>[5];
            for (var i = 0; i < _tracks.Length; i++)
            {
                _tracks[i] = new LinkedList<Classes.GameNote>();
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
        /// <summary>
        /// Starts the animation for a given note.
        /// </summary>
        /// <param name="note"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AnimateNote(Classes.GameNote note)
        {
            //TODO haser bomnito esto
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the game, initializing all components for it.
        /// </summary>
        public void StartSong()
        {
            
            _playback.StartReading();
            _stopwatch.Reset();
            _currentTime = 0L;
            _stopwatch.Start();
            var songInitThread = new Thread(StartMusicAfterOffset);
            songInitThread.Start();
            _playerThread.Start();
        }

        /// <summary>
        /// Starts playing the music after <see cref="_timeToFall"/> has passed, making the
        /// song playable.
        /// </summary>
        void StartMusicAfterOffset()
        {
            var startTime = _currentTime;
            var started = false;
            while (!started)
            {
                //Thread.Sleep((int)_timeToFall);?? Parece regulero así que mejor me quedo con el bucle más legible
                var waitedTime = _currentTime - startTime;
                if(waitedTime >= _timeToFall)
                {
                    _mediaPlayer.Play();
                    started = true;
                }
            }
        }

        /// <summary>
        /// Pauses the song and the midi reproduction.
        /// </summary>
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

        /// <summary>
        /// Resumes the song from the position it was paused at.
        /// </summary>
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

        /// <summary>
        /// Checks all the necessary song logic every system tick.
        /// </summary>
        void SongUpdate()
        {
            while (true)
            {
                _currentTime = _stopwatch.ElapsedMilliseconds;
                CheckNotesToDestroy();
            }
        }

        /// <summary>
        /// Checks all the falling notes and destroys them after the needed time.
        /// </summary>
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

        /// <summary>
        /// Checks if there's a note to play in the correct zone at the played track referenced by
        /// parameter. 
        /// Note is too far - Nothing happens
        /// Note it's close but it's further than failOffset - Failed note
        /// Note isOnTime - Correct note
        /// </summary>
        /// <param name="trackIndex"></param>
        void CheckPlayedNote(int trackIndex)
        {
            var targetTrack = _tracks[trackIndex];
            var nextNote = targetTrack.First.Value;
            if (nextNote != null)
            {
                var differenceToPerfect = Math.Abs(nextNote.MillisSinceRead - _timeToFall);//0 is perfect
                var isOnTime = differenceToPerfect <= _failOffset;
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
        /// <summary>
        /// Called whenever the user hits a key in the keyboard, calls
        /// a method to check the played track for each one of them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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
