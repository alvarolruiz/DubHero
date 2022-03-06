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

using Windows.UI;
using Windows.UI.Xaml;

using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

using Windows.UI.Xaml.Shapes;
using Windows.ApplicationModel;

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

        private readonly object _tracklock = new object();

        /// <summary>
        /// A list of tracks containing a heap of notes that are already falling.
        /// </summary>
        LinkedList<Classes.GameNote>[] _tracks;

        string _songName;
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
            _playback = new PlaybackManager();
            _playback.Dispatcher = this.Dispatcher;
            _playback.AnimationCallback += AnimateNote;
            _mediaPlayer = new MediaPlayer();
            this.InitializeComponent();
            this.Loaded += LoadSong;
            Window.Current.CoreWindow.KeyDown += MainPage_KeyDown;
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
                _songName = (string)e.Parameter;
            }
            base.OnNavigatedTo(e);
        }
        #endregion

        #region Events
        private async void LoadSong(Object sender, RoutedEventArgs e)
        {
            var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var songListFolder = await assetsFolder.GetFolderAsync("Songs");
            var songFolder = await songListFolder.GetFolderAsync(_songName);

            var mp3File = await songFolder.GetFileAsync("song.mp3");
            var midiFile = await songFolder.GetFileAsync("map.mid");
            
            await _playback.InitReader(midiFile);

            _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(mp3File.Path));

            StartSong();
        }

        #endregion

        #region Flow control
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
                    _mediaPlayer.Volume = 1;
                    _mediaPlayer.Play();
                    started = true ;
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
                lock(_tracklock) 
                {
                    CheckNotesToDestroy();
                }
            }
        }

        /// <summary>
        /// Checks all the falling notes and destroys them after the needed time.
        /// </summary>
        private async void CheckNotesToDestroy()
        {
            
            foreach (var track in _tracks)
            {
                if (track.Count > 0)
                {
                    var note = track.First();
                    if (_currentTime - note.ReadTime >= _timeToFall + _failOffset)
                    {
                        lock (_tracklock)
                        {
                            track.RemoveFirst();
                        }
                        this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            note.DeleteFromView();
                        });
                    }
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
            lock (_tracklock)
            {
                var targetTrack = _tracks[trackIndex];
                if (targetTrack.First != null)
                {
                    var nextNote = targetTrack.First.Value;
                    var timeSinceRead = _currentTime - nextNote.ReadTime;
                    var differenceToPerfect = Math.Abs(timeSinceRead - _timeToFall);//0 is perfect
                    var isOnTime = differenceToPerfect <= _failOffset;
                    if (isOnTime)
                    {
                        targetTrack.RemoveFirst();
                        nextNote.DeleteFromView();
                    }
                    else if (differenceToPerfect <= _tooSoonOffset)
                    {
                        targetTrack.RemoveFirst();
                        nextNote.DeleteFromView();
                        //TODO manage failed note
                    }
                }
            }
        }
        #endregion

        #region Animation
        /// <summary>
        /// Starts the animation for a given note.
        /// </summary>
        /// <param name="note"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AnimateNote(Classes.GameNote note)
        {
            //TODO haser bomnito esto
            note.ReadTime = _currentTime;
            generarNota(note);
            lock (_tracklock) 
            {
                _tracks[note.TrackIndex].AddLast(note);
            }
        }

        private DoubleAnimation CreateDoubleAnimation(DependencyObject frameworkElement, double fromX, double toX, string propertyToAnimate, Double interval)
        {
            DoubleAnimation animation = new DoubleAnimation();
            Storyboard.SetTarget(animation, frameworkElement);
            Storyboard.SetTargetProperty(animation, propertyToAnimate);
            animation.From = fromX;
            animation.To = toX;
            animation.Duration = TimeSpan.FromSeconds(interval);
            animation.EnableDependentAnimation = true; // no se si hay que quitarlo
            return animation;
        }





        private Storyboard crearAnimacionBajadaEncoger(Ellipse elemento, Double tiempo, int xInit, int xFin)
        {
            Storyboard storyboardTamanio = new Storyboard();
            //desde donde hasta donde quieres que se anime
            DoubleAnimation traslacionY = CreateDoubleAnimation(elemento, 100, 1000, "(Rectangle.RenderTransform).(CompositeTransform.TranslateY)", tiempo);
            storyboardTamanio.Children.Add(traslacionY);

            //desde donde hasta donde quieres que se anime
            DoubleAnimation traslacionX = CreateDoubleAnimation(elemento, xInit, xFin, "(Rectangle.RenderTransform).(CompositeTransform.TranslateX)", tiempo);
            storyboardTamanio.Children.Add(traslacionX);

            // desde que tamanio hasta que tamanio
            DoubleAnimation animacionAncho = CreateDoubleAnimation(elemento, 60, 150, "Rectangle.Width", tiempo);
            animacionAncho.EnableDependentAnimation = true;
            storyboardTamanio.Children.Add(animacionAncho);

            DoubleAnimation animacionAlto = CreateDoubleAnimation(elemento, 60, 150, "Rectangle.Height", tiempo);
            animacionAlto.EnableDependentAnimation = true;
            storyboardTamanio.Children.Add(animacionAlto);
            // anhadir animacion de traslacion en el eje x

            storyboardTamanio.Begin();

            return storyboardTamanio;
        }



        public Ellipse generarNota(Classes.GameNote nota)
        {

            //habira que hacerlas relativas a la pantalla
            int xInit = 0, xFin = 0;

            SolidColorBrush scb = new SolidColorBrush();
            switch (nota.TrackIndex)
            {
                case 0:
                    scb = new SolidColorBrush(Colors.Red); // hacer que aparezca en una coordinada 
                    xInit = 530;
                    xFin = 200;
                    break;

                case 1:
                    scb = new SolidColorBrush(Colors.Gray);
                    xInit = 600;
                    xFin = 500;
                    break;

                case 2:
                    scb = new SolidColorBrush(Colors.Pink);
                    xInit = 700;
                    xFin = 700;
                    break;

                case 3:
                    scb = new SolidColorBrush(Colors.Purple);
                    xInit = 800;
                    xFin = 1000;
                    break;

                case 4:
                    scb = new SolidColorBrush(Colors.Green);
                    xInit = 890;
                    xFin = 1200;
                    break;
            }

            Ellipse rec = new Ellipse
            {
                Width = 80,
                Height = 100 * 1.5, // esta mal pero habria que ponerlo segun la velocidad de la cancion 
                Fill = scb,
                VerticalAlignment = VerticalAlignment.Top,
                RenderTransform = new CompositeTransform { TranslateX = 0, TranslateY = 0 }
            };

            rec.SetValue(Canvas.ZIndexProperty, 6);
            crearAnimacionBajadaEncoger(rec, (_timeToFall + _failOffset)/1000, xInit, xFin);
            pistaObjetivo.Children.Add(rec);
            nota.Shape = rec;
            nota.Track = pistaObjetivo;
            return rec;
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
            
            switch (args.VirtualKey)
            {
                case VirtualKey.W: CheckPlayedNote(0); ; break;
                case VirtualKey.D: CheckPlayedNote(1); ; break;
                case VirtualKey.J: CheckPlayedNote(2); ; break;
                case VirtualKey.I: CheckPlayedNote(3); ; break;
                case VirtualKey.O: CheckPlayedNote(4); ; break;
            }
        }

            #endregion
    }
}
