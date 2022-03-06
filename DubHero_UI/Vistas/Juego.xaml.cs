
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



            //myPlayBack myPlayBack = new myPlayBack();
            generarNota(new GameNote(60, 55));
            generarNota(new GameNote(62, 55));
            generarNota(new GameNote(64, 55));
            generarNota(new GameNote(65, 55));
            generarNota(new GameNote(67, 55));

            generarNota(new GameNote(60, 55));
            generarNota(new GameNote(62, 55));
            generarNota(new GameNote(64, 55));
            generarNota(new GameNote(65, 55));
            generarNota(new GameNote(67, 55));

        }

        protected override void OnNavigatedTo(NavigationEventArgs e) // para recuperar los parametros 
        {
            base.OnNavigatedTo(e);

            //String nombre = (RestaurantParams)e.Parameter;

            // parameters.Name
            // parameters.Text
            // ...
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
            DoubleAnimation animacionAncho = CreateDoubleAnimation(elemento, 60, 150, "Rectangle.Width", tiempo);
            animacionAncho.EnableDependentAnimation = true;
            storyboardTamanio.Children.Add(animacionAncho);

            DoubleAnimation animacionAlto = CreateDoubleAnimation(elemento, 60, 150, "Rectangle.Height", tiempo);
            animacionAlto.EnableDependentAnimation = true;
            storyboardTamanio.Children.Add(animacionAlto);
            // anhadir animacion de traslacion en el eje x

            storyboardTamanio.RepeatBehavior = RepeatBehavior.Forever;
            storyboardTamanio.Begin();
        }



        public Ellipse generarNota(GameNote nota)
        {

            //habira que hacerlas relativas a la pantalla
            int tipo = 0, xInit = 0, xFin = 0;

            SolidColorBrush scb = new SolidColorBrush();
            switch (nota.Type)
            {
                case 60:
                    scb = new SolidColorBrush(Colors.Red); // hacer que aparezca en una coordinada 
                    xInit = 390;
                    xFin = 60;
                    break;

                case 62:
                    scb = new SolidColorBrush(Colors.Gray);
                    xInit = 460;
                    xFin = 300;
                    break;

                case 64:
                    scb = new SolidColorBrush(Colors.Pink);
                    xInit = 550;
                    xFin = 550;
                    break;

                case 65:
                    scb = new SolidColorBrush(Colors.Purple);
                    xInit = 630;
                    xFin = 830;
                    break;

                case 67:
                    scb = new SolidColorBrush(Colors.Green);
                    xInit = 720;
                    xFin = 1060;
                    break;
            }

            Ellipse rec = new Ellipse
            {
                Width = 80,
                Height = nota.Duration * 1.5, // esta mal pero habria que ponerlo segun la velocidad de la cancion 
                Fill = scb,
                //CornerRadius = 5,
                VerticalAlignment = VerticalAlignment.Top,
                RenderTransform = new CompositeTransform { TranslateX = 0, TranslateY = 0 }

            };

            rec.SetValue(Canvas.ZIndexProperty, 6);

            crearAnimacionBajadaEncoger(rec, nota.Duration / 30, xInit, xFin);
            pistaObjetivo.Children.Add(rec);
            return rec;
        }
        }


        public class GameNote
    {
        private int type;
        private float duration;

        public GameNote(int tipo, float duration)
        {

            this.type = tipo;
            this.duration = duration;

        }



        public int Type { get => type; set => type = value; }
        public float Duration { get => duration; set => duration = value; }


    }



    public class myPlayBack
    {
        private Playback reproductorMidi;
        private MidiFile midiFile;
        private SevenBitNumber note;




        public myPlayBack()
        {
            midiFile = MidiFile.Read("Assets/sevenNationArmyMap.mid");
            this.reproductorMidi = midiFile.GetPlayback();
            this.reproductorMidi.NoteCallback += EventoNota;



        }

        public void empezarMidi()
        {

            reproductorMidi.Start();

        }


        private NotePlaybackData EventoNota(NotePlaybackData rawData, long rawTime, long rawLength, TimeSpan playbackTime)
        {


            return rawData;

        }


        private void ReproductorMidi_EventPlayed(object sender, MidiEventPlayedEventArgs e)
        {

            if (e.Event.EventType == MidiEventType.NoteOn)
            {


            }
            var evento = (NoteEvent)e.Event;
            int numero = evento.NoteNumber;

        }

        public Playback ReproductorMidi { get => reproductorMidi; set => reproductorMidi = value; }




        public SevenBitNumber Note { get => note; set => note = value; }







    }
}










