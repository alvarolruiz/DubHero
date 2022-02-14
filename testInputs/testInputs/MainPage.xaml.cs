using System;
using System.Windows;
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
using System.Drawing;
using Windows.UI;
using Windows.UI.Core;
using System.Windows.Input;
using System.Diagnostics;
using Windows.System;
using System.Timers;
using Windows.UI.Xaml.Media.Animation;
// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace testInputs
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private bool _Q;
        private bool _D;
        private bool _H;
        private bool _I;
        private bool _P;

        private bool _mash;


        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += MainPage_KeyDown;
            Window.Current.CoreWindow.KeyUp += MainPage_KeyUp;
            miStoryboard.Begin();
        }



 


        private void cambiarColores()
        {
            if (_Q) 
                espacio1.Fill = new SolidColorBrush(Colors.Red);
            else
                espacio1.Fill = new SolidColorBrush(Colors.White);

            if (_D)
                espacio2.Fill = new SolidColorBrush(Colors.Blue);
            else
                espacio2.Fill = new SolidColorBrush(Colors.White);

            if (_H) 
                espacio3.Fill = new SolidColorBrush(Colors.Green);
            else
                espacio3.Fill = new SolidColorBrush(Colors.White);

            if (_I) 
                espacio4.Fill = new SolidColorBrush(Colors.Yellow);
            else
                espacio4.Fill = new SolidColorBrush(Colors.White);

            if (_P) 
                espacio5.Fill = new SolidColorBrush(Colors.Orange);
            else
                espacio5.Fill = new SolidColorBrush(Colors.White);

            if (_mash)
            {
                espacio6.Fill = new SolidColorBrush(Colors.Black);
                chechMashing(_Q, _D, _H, _I,_P);
            }
            else
                espacio6.Fill = new SolidColorBrush(Colors.White);
        }

        private void chechMashing(bool q, bool d, bool h, bool i, object p)
        {
            String textoMash = "Se están masheando las teclas: ";
            if (_Q)
                textoMash = textoMash + "Q";
 
            if (_D)
                textoMash = textoMash + " D";

            if (_H)
                textoMash = textoMash + " H";

            if (_I)
                textoMash = textoMash + " I";

            if (_P)
                textoMash = textoMash + " P";

            noitif.Text = textoMash;
        }

        public void MainPage_KeyDown(object sender, KeyEventArgs args)
        {
            
            if (args.VirtualKey == VirtualKey.Q)            
                _Q = true;
            
            if (args.VirtualKey == VirtualKey.D)            
                _D = true;
            
            if (args.VirtualKey == VirtualKey.H)            
                _H = true;
            
            if (args.VirtualKey == VirtualKey.I)            
                _I = true;
            
            if (args.VirtualKey == VirtualKey.P)            
                _P = true;

            if (args.VirtualKey == VirtualKey.Space)
                _mash = true;

            cambiarColores();
        }

        public void MainPage_KeyUp(object sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Q)
                _Q = false;

            if (args.VirtualKey == VirtualKey.D)
                _D = false;

            if (args.VirtualKey == VirtualKey.H)
                _H = false;

            if (args.VirtualKey == VirtualKey.I)
                _I = false;

            if (args.VirtualKey == VirtualKey.P)
                _P = false;

            if (args.VirtualKey == VirtualKey.Space)
                _mash = false;

            cambiarColores();
        }



       
    }
}
