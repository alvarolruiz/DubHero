using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace DubHero_UI.Vistas
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region 
        private bool firstTime = true;
        private MediaPlayer mediaPlayer;
#endregion


        public MainPage()
        {

            this.InitializeComponent();
            pulse.Begin();
            pulse2.Begin();

            mediaPlayer = new MediaPlayer();
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Songs/Rubadub/Rubadub.mp3"));
            var session = mediaPlayer.PlaybackSession;
            session.Position = session.Position + TimeSpan.FromSeconds(41);
            if (firstTime)
            {
                mediaPlayer.Play();
            }
        }


        void Grid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SelectSong), mediaPlayer);
            firstTime = false;
        }

        private void viewBtn_Click(object sender, RoutedEventArgs e)
        {
            
            mediaPlayer.Pause();
            Frame.Navigate(typeof(SelectSong), mediaPlayer);
        }



        protected override void OnNavigatedTo(NavigationEventArgs e) // para recuperar los parametros 
        {
            if (e.Parameter == "False")
            {
                firstTime = false;
            }


            base.OnNavigatedTo(e);



            //String nombre = (RestaurantParams)e.Parameter;

            // parameters.Name
            // parameters.Text
            // ...
        }

    }
}