
using DubHero_UI.Classes;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace DubHero_UI.Vistas
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Juego : Page
    {
        //private PlaybackManager _playback;

        public Juego()
        {
            this.InitializeComponent();

            succesAnimation1.Begin();
            succesAnimation2.Begin();
            succesAnimation3.Begin();
            succesAnimation4.Begin();
            succesAnimation5.Begin();
            failAnimation1.Begin();
            failAnimation2.Begin();
            failAnimation3.Begin();
            failAnimation4.Begin();
            failAnimation5.Begin();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e) // para recuperar los parametros 
        {
            base.OnNavigatedTo(e);

            //String nombre = (RestaurantParams)e.Parameter;

            // parameters.Name
            // parameters.Text
            // ...
        }
    }
}










