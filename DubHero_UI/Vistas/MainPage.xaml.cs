using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace DubHero_UI.Vistas
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region 
       
        private MediaPlayer mediaPlayer;
        private MediaTimelineController _mediaTimelineController;

#endregion


        public MainPage()
        {

            this.InitializeComponent();
            pulse.Begin();
            pulse2.Begin();

            mediaPlayerInit();
        }


        /// <summary>
        /// Inicia la cancion que va a sonar de fondo de todo el programa
        /// </summary>
        private void mediaPlayerInit()
        {
   

            mediaPlayer = new MediaPlayer();
            //mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Songs/Rubadub/Rubadub.mp3"));
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Songs/cristina.mp3"));
            _mediaTimelineController = new MediaTimelineController();
            mediaPlayer.CommandManager.IsEnabled = false;
            mediaPlayer.TimelineController = _mediaTimelineController;
           
            _mediaTimelineController.Start();
            
        }

        /// <summary>
        /// Cambia a la pantalla siguiente 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Grid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SelectSong), _mediaTimelineController);
        }


        /// <summary>
        /// Pausa o renuda la cancion que se inicia al principio de la aplcacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void viewBtn_Click(object sender, RoutedEventArgs e)
        {
            var yoquerese = mediaPlayer.PlaybackSession.PlaybackState;

            if (_mediaTimelineController.State == MediaTimelineControllerState.Running) {
                _mediaTimelineController.Pause();
            }
            else {
                _mediaTimelineController.Resume();
            }
            
        }


        /// <summary>
        /// Sale de toda la aplicacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnexit_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }
    }
}