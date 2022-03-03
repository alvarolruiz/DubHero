
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

            


            do {
                succesAnimation.Begin();
                succesAnimation2.Begin();
                succesAnimation3.Begin();
                succesAnimation4.Begin();
                succesAnimation5.Begin();
                failAnimation.Begin();
                failAnimation2.Begin();
                failAnimation3.Begin();
                failAnimation4.Begin();
                failAnimation5.Begin();

            } while (true);

            //myPlayBack myPlayBack = new myPlayBack();
            //generarNota(new GameNote(67, 55));
            //generarNota(new GameNote(65, 100));
            //generarNota(new GameNote(64, 55));

        }

        protected override void OnNavigatedTo(NavigationEventArgs e) // para recuperar los parametros 
        {
            base.OnNavigatedTo(e);

            //String nombre = (RestaurantParams)e.Parameter;

            // parameters.Name
            // parameters.Text
            // ...
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





        private void crearAnimacionBajadaEncoger(Rectangle elemento, Double tiempo, int xInit,int xFin)
        {
            Storyboard storyboardTamanio = new Storyboard();
            //desde donde hasta donde quieres que se anime
            DoubleAnimation traslacionY = CreateDoubleAnimation(elemento, 0, 800, "(Rectangle.RenderTransform).(CompositeTransform.TranslateY)", tiempo);
            storyboardTamanio.Children.Add(traslacionY);

            //desde donde hasta donde quieres que se anime
            DoubleAnimation traslacionX = CreateDoubleAnimation(elemento, xInit, xFin, "(Rectangle.RenderTransform).(CompositeTransform.TranslateX)", tiempo);
            storyboardTamanio.Children.Add(traslacionX);

            // desde que tamanio hasta que tamanio
            DoubleAnimation animacionTamanio = CreateDoubleAnimation(elemento, 115, 60, "Rectangle.Width", tiempo);
            animacionTamanio.EnableDependentAnimation = true;
            storyboardTamanio.Children.Add(animacionTamanio);
            // anhadir animacion de traslacion en el eje x


            storyboardTamanio.Begin();
        }



        public Rectangle generarNota(GameNote nota)
        {
            //String nombrePista = (string)pistaObjetivo.GetType().GetProperty("Name").GetValue(pistaObjetivo, null);
            int tipo  = 0, xInit=0, xFin=0;

            SolidColorBrush scb = new SolidColorBrush();
            switch (nota.Type)
            {
                case 60:
                    scb = new SolidColorBrush(Colors.Red); // hacer que aparezca en una coordinada 
                    xInit = 200;
                    xFin = 100;
                    break;

                case 62:
                    scb = new SolidColorBrush(Colors.Gray);
                    xInit = 400;
                    xFin = 100;
                    break;

                case 64:
                    scb = new SolidColorBrush(Colors.Pink);
                    xInit = 500;
                    xFin = 100;
                    break;

                case 65:
                    scb = new SolidColorBrush(Colors.Purple);
                    xInit = 600;
                    xFin = 100;
                    break;

                case 67:
                    scb = new SolidColorBrush(Colors.Green);
                    xInit = 700;
                    xFin = 100;
                    break;
            }

            Rectangle rec = new Rectangle
            {
                Width = 80,
                Height = nota.Duration * 2, // esta mal pero habria que ponerlo segun la velocidad de la cancion 
                Fill = scb,
                VerticalAlignment = VerticalAlignment.Top,
                RenderTransform = new CompositeTransform { TranslateX = 0, TranslateY = 0 }
            };
            crearAnimacionBajadaEncoger(rec, nota.Duration, xInit, xFin);
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


            //(rawNoteData, rawTime, rawLength, playbackTime) =>

            // new NotePlaybackData(
            // rawNoteData.NoteNumber,
            // rawNoteData.Velocity,     // leave velocity as is
            // rawNoteData.OffVelocity,  // leave off velocity as is
            // rawNoteData.Channel);
            // reproductorMidi.EventPlayed += ReproductorMidi_EventPlayed;
            // reproductorMidi.TrackNotes = true;
            

        }

        public void empezarMidi() {

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










