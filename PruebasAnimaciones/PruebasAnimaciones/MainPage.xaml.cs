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
