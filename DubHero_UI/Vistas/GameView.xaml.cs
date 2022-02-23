using DubHero_UI.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace DubHero_UI.Vistas
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class GameView : Page
    {
        MediaPlayer _mediaPlayer = new MediaPlayer();
        Stopwatch _stopwatch = new Stopwatch();
        PlaybackManager _playback;

        long timeToFall = 3000L;
        long failOffset = 0L;
        long _currentTime = 0;
        Thread _playerThread;

        LinkedList<GameNote>[] _tracks;

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
            _currentTime += _stopwatch.ElapsedMilliseconds;
            CheckNotesToPush();
        }

        private void CheckNotesToPush()
        {
            foreach (var track in _tracks)
            {
                var note = track.First();
                if (note.MillisSinceRead >= timeToFall + failOffset)
                {
                    //Destroy note in the view
                    track.Remove(note);
                }
            }
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



        
    }
}
