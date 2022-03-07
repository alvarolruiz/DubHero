
using DubHero_UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

        public SelectSong()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// funcion que navega a la pantalla de juego tras seleccionar una cancion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            String songFolder = ((SongView)e.ClickedItem).FolderName;
            Frame.Navigate(typeof(GameView), songFolder);

        }
        /// <summary>
        /// funcion que navega a la pantalla principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}



