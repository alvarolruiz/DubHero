using DubHero_UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
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
    public sealed partial class Tutorial : Page
    {
        private MediaTimelineController _mediaTimelineController;
        private TutorialWrapper tutorialWrapper;

        public Tutorial()
        {
            this.InitializeComponent();
            pulse.Begin();
            pulse2.Begin();
        }




        /// <summary>
        /// Recoge parametros de otra pantalla
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e) // para recuperar los parametros 
        {
            base.OnNavigatedTo(e);
            tutorialWrapper = e.Parameter as TutorialWrapper;
            _mediaTimelineController = tutorialWrapper.MediaTimelineController;
           


        }


        /// <summary>
        /// Pausa o renuda la cancion que se inicia al principio de la aplcacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewBtn_Click(object sender, RoutedEventArgs e)
        {


            if (_mediaTimelineController.State == MediaTimelineControllerState.Running)
            {
                _mediaTimelineController.Pause();
            }
            else
            {
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


        /// <summary>
        /// va a la sguiente pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RelativePanel_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            _mediaTimelineController.Pause();
            Frame.Navigate(typeof(Juego), tutorialWrapper.SongName);
        }
    }
}
