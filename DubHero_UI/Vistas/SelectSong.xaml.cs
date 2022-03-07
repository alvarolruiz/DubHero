
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
    public sealed partial class SelectSong : Page
    {
        private MediaTimelineController _mediaTimelineController;

        public SelectSong()
        {
            this.InitializeComponent();
            pulse.Begin();
        }


        /// <summary>
        /// funcion que navega a la pantalla de juego tras seleccionar una cancion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            String cancion = ((SongView)e.ClickedItem).Name;
            TutorialWrapper wrap = new TutorialWrapper(cancion, _mediaTimelineController);
            Frame.Navigate(typeof(Tutorial), wrap);

        }


        /// <summary>
        /// Recoge parametros de otra pantalla
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e) // para recuperar los parametros 
        {
            base.OnNavigatedTo(e);
            _mediaTimelineController = e.Parameter as MediaTimelineController;
            if (_mediaTimelineController.State != MediaTimelineControllerState.Running)
            {

                _mediaTimelineController.Resume();
              
            }
         

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


    }
}



