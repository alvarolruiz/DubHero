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
using DubHero_UI.Models;
using Windows.UI.Xaml.Media.Imaging;

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

        /// <summary>
        /// Name of the song, used to fing the necessary files.
        /// </summary>
        SongView songView;

        /// <summary>
        /// Tracks current score
        /// </summary>
        Score _scoreboard;
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
            if (e.Parameter is SongView)
            {
                songView = (SongView)e.Parameter;
            }
            base.OnNavigatedTo(e);
            initFailSuccesAnimation();
        }
        #endregion

        #region Events
        /// <summary>
        /// Loads async all the data required for the game (song and midi files, score with total amount of notes)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LoadSong(Object sender, RoutedEventArgs e)
        {
            var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            var songListFolder = await assetsFolder.GetFolderAsync("Songs");
            var songFolder = await songListFolder.GetFolderAsync(songView.FolderName);
            var mp3File = await songFolder.GetFileAsync("song.mp3");
            var midiFile = await songFolder.GetFileAsync("map.mid");
            this.nombreCancion.Text = songView.Name;
            this.artista.Text = songView.Artist;
            this.dificulty.Value = songView.Dificulty;

            await _playback.InitReader(midiFile);

            _mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(mp3File.Path));
            _mediaPlayer.MediaEnded += SongEnded;

            _scoreboard = new Score();
            _scoreboard.ScoreText = this.ScoreText;
            _scoreboard.CorrectValue = 1000000 / _playback.GetNoteQuantity();

            StartSong();
        }

        /// <summary>
        /// Navigates to results page after the song ends.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SongEnded(MediaPlayer sender, object args)
        {
            this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Frame.Navigate(typeof(Results), _scoreboard);
            });
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
                if (waitedTime >= _timeToFall + 700)//Los 700 es una regulación a mano, corregir el midi o animaciones
                {
                    _mediaPlayer.Volume = 1;
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
                lock (_tracklock)
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
                            animateFail(note);
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


                        this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            animateSucces(nextNote);
                            nextNote.DeleteFromView();
                            
                        });
                        targetTrack.RemoveFirst();
                        _scoreboard.Correct();


                    }
                    else if (differenceToPerfect <= _tooSoonOffset)
                    {
                        this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            animateFail(nextNote);
                            nextNote.DeleteFromView();
                        });
                        targetTrack.RemoveFirst();
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



        /// <summary>
        /// Execute all of the animations
        /// </summary>
        private void initFailSuccesAnimation()
        {

            failAnimation1.Begin();
            failAnimation2.Begin();
            failAnimation3.Begin();
            failAnimation4.Begin();
            failAnimation5.Begin();
            succesAnimation1.Begin();
            succesAnimation2.Begin();
            succesAnimation3.Begin();
            succesAnimation4.Begin();
            succesAnimation5.Begin();
        }

        /// <summary>
        /// Lanza una animacion de fuego segun la nota que se haya acertado
        /// </summary>
        /// <param name="note"></param>
        private void animateSucces(Classes.GameNote note)
        {

            switch (note.NoteNumber)
            {
                case 60:
                    succesAnimation1.Begin();

                    break;

                case 62:
                    succesAnimation2.Begin();

                    break;

                case 64:
                    succesAnimation3.Begin();

                    break;

                case 65:
                    succesAnimation4.Begin();

                    break;

                case 67:
                    succesAnimation5.Begin();

                    break;
            }

        }
        /// <summary>
        /// Lanza una animacion roja segun la nota que se haya fallado
        /// </summary>
        /// <param name="note"></param>

        private void animateFail(Classes.GameNote note)
        {

            switch (note.NoteNumber)
            {
                case 60:
                    failAnimation1.Begin();

                    break;

                case 62:
                    failAnimation2.Begin();

                    break;

                case 64:
                    failAnimation3.Begin();

                    break;

                case 65:
                    failAnimation4.Begin();

                    break;

                case 67:
                    failAnimation5.Begin();

                    break;
            }

        }



        /// <summary>
        /// funcion que crea y devuelve una animacion doble dado un rctangulo y una serie de parametros
        /// </summary>
        /// <param name="frameworkElement">Elemento a animar</param>
        /// <param name="fromX">posicion inicial</param>
        /// <param name="toX"> posicion final</param>
        /// <param name="propertyToAnimate">que especto del elmento se quiere animar</param>
        /// <param name="interval">duracion de la animacion</param>
        /// <returns>una animacion con los parametros introducidos</returns>
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


        /// <summary>
        /// Crea una animacion sobre un elemnto de la vista pasado por parametreos
        /// </summary>
        /// <param name="elemento"></param>
        /// <param name="tiempo"></param>
        /// <param name="xInit"></param>
        /// <param name="xFin"></param>


        private void crearAnimacionBajadaEncoger(Ellipse elemento, Double tiempo, int xInit, int xFin)
        {

            double timeFinishLine = (tiempo / 800) * 200;
            tiempo += timeFinishLine;

            Storyboard storyboardTamanio = new Storyboard(); // tiene que moveerse 200p mas
            //desde donde hasta donde quieres que se anime
            DoubleAnimation traslacionY = CreateDoubleAnimation(elemento, 100, 1100, "(Rectangle.RenderTransform).(CompositeTransform.TranslateY)", tiempo);
            storyboardTamanio.Children.Add(traslacionY);

            //desde donde hasta donde quieres que se anime
            DoubleAnimation traslacionX = CreateDoubleAnimation(elemento, xInit, xFin, "(Rectangle.RenderTransform).(CompositeTransform.TranslateX)", tiempo);
            storyboardTamanio.Children.Add(traslacionX);

            // desde que tamanio hasta que tamanio
            DoubleAnimation animacionAncho = CreateDoubleAnimation(elemento, 40, 180, "Rectangle.Width", tiempo);
            animacionAncho.EnableDependentAnimation = true;
            storyboardTamanio.Children.Add(animacionAncho);

            DoubleAnimation animacionAlto = CreateDoubleAnimation(elemento, 40, 180, "Rectangle.Height", tiempo);
            animacionAlto.EnableDependentAnimation = true;
            storyboardTamanio.Children.Add(animacionAlto);

            storyboardTamanio.Begin();
        }



        /// <summary>
        /// Genera una elipse con los datos de la nota pasada, que luega se anima y se muestra en pantalla
        /// </summary>
        /// <param name="nota"></param>
        /// <returns></returns>
        public Ellipse generarNota(GameNote nota)
        {

            //habira que hacerlas relativas a la pantalla
            int xInit = 0, xFin = 0;

            SolidColorBrush scb = new SolidColorBrush();
            switch (nota.NoteNumber)
            {
                case 60:
                    scb = new SolidColorBrush(Colors.Red); // hacer que aparezca en una coordinada 
                    xInit = 380;
                    xFin = 15;
                    break;

                case 62:
                    scb = new SolidColorBrush(Colors.Gray);
                    xInit = 468;
                    xFin = 286;
                    break;

                case 64:
                    scb = new SolidColorBrush(Colors.Pink);
                    xInit = 564;
                    xFin = 552;
                    break;

                case 65:
                    scb = new SolidColorBrush(Colors.Purple);
                    xInit = 665;
                    xFin = 835;
                    break;

                case 67:
                    scb = new SolidColorBrush(Colors.Green);
                    xInit = 760;
                    xFin = 1091;
                    break;
            }


            ImageBrush imgB = new ImageBrush();

            BitmapImage btpImg = new BitmapImage();

            btpImg.UriSource = new Uri(@"ms-appx:///Assets/Imagenes/Notas/azul.png");

            imgB.ImageSource = btpImg;



            Ellipse rec = new Ellipse
            {
                Width = 80,
                Height = (_currentTime - nota.ReadTime) * 1.5, // esta mal pero habria que ponerlo segun la velocidad de la cancion 
                Fill = imgB,

                //CornerRadius = 5,
                VerticalAlignment = VerticalAlignment.Top,
                RenderTransform = new CompositeTransform { TranslateX = 0, TranslateY = 0 }


            };

            rec.SetValue(Canvas.ZIndexProperty, 6);
            nota.Shape = rec;
            nota.Track = pistaObjetivo;
            crearAnimacionBajadaEncoger(rec, (double)(_timeToFall + _failOffset) / 1000, xInit, xFin);
            pistaObjetivo.Children.Add(rec);
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
