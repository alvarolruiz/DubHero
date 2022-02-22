using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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


namespace PruebasAnimaciones
{

    public sealed partial class MainPage : Page
    {


        /**todo: -crear y destuir objeto y animacion desde el viewModel parametros(tipo de nota, duracion), hacer un binding al Canva??? o hacer 5 listas, donde hacer
         * aparecer el objeto(si le cmabiamos la animacion se veria), y borrarlo luego. Tendria la animacion si esta en una lista??
       *              cambiar icono a una nota ya creada, crear 5 variable string, notaActual que guardara el Name de la nota que esta bajando,
       *              cada nota actual, tendra su timmer(= que el de animacion), cuando el timmer se exceda  */




        public class myPlayBack
        {
            private Playback reproductorMidi;
            private MidiFile midiFile;

            public myPlayBack()
            {
                midiFile = MidiFile.Read("Assets/sevenNationArmyMap.mid");
                this.reproductorMidi = midiFile.GetPlayback();
                this.reproductorMidi.NoteCallback =
               (rawNoteData, rawTime, rawLength, playbackTime) =>

                new NotePlaybackData(

                nota(rawNoteData, playbackTime),
                rawNoteData.Velocity,     // leave velocity as is
                rawNoteData.OffVelocity,  // leave off velocity as is
                rawNoteData.Channel);


                reproductorMidi.TrackNotes = true;
                reproductorMidi.Start()
               ;

            }

            private SevenBitNumber nota(NotePlaybackData rawNoteData, TimeSpan playbackTime)
            {
                return rawNoteData.NoteNumber;
            }



        }



        public MainPage()
        {
            this.InitializeComponent();
            crearAnimacionBajadaEncoger(crearElementoEn(pista1));
            crearAnimacionBajadaEncoger(crearElementoEn(pista2));
            crearAnimacionBajadaEncoger(crearElementoEn(pista3));
            crearAnimacionBajadaEncoger(crearElementoEn(pista4));
            crearAnimacionBajadaEncoger(crearElementoEn(pista5));
        }
        public void crearAnimacionBajadaEncoger(Rectangle elemento)
        {
            Storyboard storyboardTamanio = new Storyboard();
            DoubleAnimation animateScaleX = CreateDoubleAnimation(elemento, 0, 800, "(Rectangle.RenderTransform).(CompositeTransform.TranslateY)", 1.5);
            storyboardTamanio.Children.Add(animateScaleX);
            DoubleAnimation animacionTamanio = CreateDoubleAnimation(elemento, 115, 60, "Rectangle.Width", 1.5);
            animacionTamanio.EnableDependentAnimation = true;
            storyboardTamanio.Children.Add(animacionTamanio);
            storyboardTamanio.Begin();
        }

        private DoubleAnimation CreateDoubleAnimation(DependencyObject frameworkElement, double fromX, double toX, string propertyToAnimate, Double interval)
        {
            DoubleAnimation animation = new DoubleAnimation();
            Storyboard.SetTarget(animation, frameworkElement);
            Storyboard.SetTargetProperty(animation, propertyToAnimate);
            animation.From = fromX;
            animation.To = toX;
            animation.Duration = TimeSpan.FromSeconds(interval);
            animation.EnableDependentAnimation=true;
            return animation;
        }

        public Rectangle crearElementoEn(Grid pistaObjetivo)
        {
            String nombrePista = (string)pistaObjetivo.GetType().GetProperty("Name").GetValue(pistaObjetivo, null);
            SolidColorBrush scb = new SolidColorBrush();
            switch (nombrePista)
            {
                case "pista1":
                    scb = new SolidColorBrush(Colors.Red);
                    break;
                case "pista2":
                    scb = new SolidColorBrush(Colors.Gray);
                    break;
                case "pista3":
                    scb = new SolidColorBrush(Colors.Pink);
                    break;
                case "pista4":
                    scb = new SolidColorBrush(Colors.Purple);
                    break;
                case "pista5":
                    scb = new SolidColorBrush(Colors.Green);
                    break;
            }

            Rectangle rec = new Rectangle
            {
                Width = 80,
                Height = 30,
                Fill = scb,
                VerticalAlignment = VerticalAlignment.Top,
                RenderTransform = new CompositeTransform { TranslateX = 0 }
            };

            pistaObjetivo.Children.Add(rec);
            return rec;
        }










    }
}
