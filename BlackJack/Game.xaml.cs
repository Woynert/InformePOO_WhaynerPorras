using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlackJack
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        public BitmapImage imgCorazon = new BitmapImage(new Uri(@"Resources/Corazon.png", UriKind.Relative));

        public Game()
        {
            InitializeComponent();
            //iniciar
        }

        private void btnPedir_Click(object sender, RoutedEventArgs e) //Pedir
        {
            imgImage.Source = imgCorazon;// new BitmapImage(new Uri(@"Resources/Corazon.png", UriKind.Relative));
        }

        private int Check(List<Card> hand) //Sacar Valor
        {
            int val = 0;
            for(int i = 0; i < hand.Count; i++)
            {
                val += hand[i].Score;
            }
            return (val);
        }

        private void btnPlantar_Click(object sender, RoutedEventArgs e) //Plantarse
        {
            imgImagePlace.Source = new BitmapImage(new Uri(@"Resources/rect1678-5-1.png", UriKind.Relative));
            DrawCard();
        }

        private void DrawCard()
        {
            //Crear color
            //Color Color1 = new Color(); Color1.A = 255; Color1.R = 255; Color1.G = Color1.R; Color1.B = Color1.R;
            //Color Color2 = new Color(); Color2.A = 255; Color2.R = 225; Color2.G = Color2.R; Color2.B = Color2.R;
            Color Color3 = new Color(); Color3.A = 255; Color3.R = 207; Color3.G = Color3.R; Color3.B = Color3.R;
            Color Color4 = new Color(); Color4.A = 255; Color4.R = 255; Color4.G = 0; Color4.B = 0;

            //LinearGradientBrush newColor = new LinearGradientBrush(Color1, Color2, new System.Windows.Point(0, 0), new System.Windows.Point(0, 2));
            SolidColorBrush newColor2 = new SolidColorBrush(Color3);
            rctTest2.Width = 121;
            rctTest2.Height = 189;
            rctTest2.Stroke = newColor2;

            //BitmapImage img = new BitmapImage(new Uri(@"Resources/Corazon.png", UriKind.Relative));

            rctTest2.Fill = new ImageBrush { ImageSource = imgImagePlace.Source};
            rctTest2.StrokeThickness = 3;
            rctTest2.RadiusX = 10;
            rctTest2.RadiusY = rctTest.RadiusX;

            lblTest2.FontFamily = new FontFamily("Source Sans Pro");
            lblTest2.FontSize = 40;
            lblTest2.Content = "11";
            lblTest2.Foreground = new SolidColorBrush(Color4);
            Thickness newMargin = lblTest2.Margin;
            newMargin.Left = rctTest2.Margin.Left; newMargin.Top = rctTest2.Margin.Top;
            lblTest2.Margin = newMargin;

            newMargin = imgTest2.Margin;
            newMargin.Left = rctTest2.Margin.Left + (rctTest2.Width/2) - (imgTest2.Width/2); newMargin.Top = rctTest2.Margin.Top + (rctTest2.Height / 2) - (imgTest2.Height / 2);
            imgTest2.Margin = newMargin;
            imgTest2.Width = 110; imgTest2.Height = imgTest2.Width;


        }
    }
}

