using BlackJack.Classes;
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
using System.Windows.Media.Animation;
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
        Dealer dealer = new Dealer();

        public Game()
        {
            InitializeComponent();
            //iniciar
            dealer.Init();
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

            int cont = 0;
            foreach (Card card in dealer.deck)
            {
                //DrawCard(0, 0, card.symbol, card.suit);
                DrawCard(150 * (cont - card.suit * 13), 200 * card.suit, card.symbol, card.suit);
                cont++;
            }
            //DrawCard(10, 100, 13, 3);
            //DrawCard(110, 100, 11, 0);
            //DrawCard(210, 100, 4, 1);
        }

        private void DrawCard(int x, int y, int symbol, int suit)
        {
           
            int wt = 121;
            int ht = 189;
            y += -Convert.ToInt32(grdMaster.ActualHeight) + ht;
            x += -Convert.ToInt32(grdMaster.ActualWidth) +wt;

            //Color borde
            Color ColorStroke = new Color(); ColorStroke.A = 255; ColorStroke.R = 207; ColorStroke.G = ColorStroke.R; ColorStroke.B = ColorStroke.R;

            Color ColorValue;
            //Color Letra
            if (suit > 1) //Trevor, Pica
            {
                ColorValue = new Color(); ColorValue.A = 255; ColorValue.R = 0; ColorValue.G = 0; ColorValue.B = 0;
            }
            else //Corazon, Diamante
            {
                ColorValue = new Color(); ColorValue.A = 255; ColorValue.R = 255; ColorValue.G = 0; ColorValue.B = 0;
            }

            //Imagen a mostrar
            BitmapImage btmIcon;
            switch (suit)
            {
                case 0:
                    btmIcon = new BitmapImage(new Uri(@"Resources/Corazon.png", UriKind.Relative));
                    break;
                case 1:
                    btmIcon = new BitmapImage(new Uri(@"Resources/Dimante.png", UriKind.Relative));
                    break;
                case 2:
                    btmIcon = new BitmapImage(new Uri(@"Resources/Trevor.png", UriKind.Relative));
                    break;
                case 3:
                    btmIcon = new BitmapImage(new Uri(@"Resources/Pica.png", UriKind.Relative));
                    break;
                default:
                    btmIcon = new BitmapImage(new Uri(@"Resources/MissingNumber.png", UriKind.Relative));
                    break;
            }
            

            String Sym;

            //Letra a mostrar
            switch (symbol)
            {
                case 1:  Sym = "A"; break;
                case 11: Sym = "J"; break;
                case 12: Sym = "Q"; break;
                case 13: Sym = "K"; break;
                default: Sym = symbol.ToString(); break;
            }

            //Crear nuevos controles
            Grid      grdNew = new Grid();
            Rectangle rctNew = new Rectangle();
            Label     lblNew = new Label();
            Image     imgNew = new Image();

            grdMaster.Children.Add(grdNew);

            //Grid Base
            Thickness marginGrid = new Thickness(x, y, 0, 0);
            grdNew.Margin = marginGrid;
            grdNew.Width = wt;
            grdNew.Height = ht;
            grdNew.Children.Add(rctNew);
            grdNew.Children.Add(lblNew);
            grdNew.Children.Add(imgNew);

            //Carta (rectangulo)
            Thickness marginCard = new Thickness(0, 0, 0, 0);
            rctNew.Margin = marginCard;

            rctNew.Width = wt;
            rctNew.Height = ht;
            rctNew.Stroke =  new SolidColorBrush(ColorStroke);

            rctNew.Fill = new ImageBrush { ImageSource = imgImagePlace.Source};
            rctNew.StrokeThickness = 3;
            rctNew.RadiusX = 10;
            rctNew.RadiusY = rctTest.RadiusX;

            //Numero o Valor
            lblNew.FontFamily = new FontFamily("Source Sans Pro");
            lblNew.FontSize = 45;
            lblNew.Content = Sym;
            lblNew.Foreground = new SolidColorBrush(ColorValue);

            Thickness marginValue = new Thickness(rctNew.Margin.Left + 7, rctNew.Margin.Top, 0, 0);
            lblNew.Margin = marginValue;

            //Icono
            imgNew.Width = 89; imgNew.Height = imgNew.Width;
            imgNew.Source = btmIcon;// new BitmapImage(new Uri(@"Resources/Pica.png", UriKind.Relative));

        }

        private void Animate(Control obj)
        {
            //Crear transforms
            RotateTransform myRotateTransform = new RotateTransform();
            TranslateTransform myTranslate = new TranslateTransform();
            ScaleTransform myScaleTransform = new ScaleTransform();

            //Asignar
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(myTranslate);
            myTransformGroup.Children.Add(myScaleTransform);
            myTransformGroup.Children.Add(myRotateTransform);
            obj.RenderTransform = myTransformGroup;

            //Animar
            DoubleAnimation aniX = new DoubleAnimation(0, 1000, TimeSpan.FromSeconds(5));
            DoubleAnimation aniY = new DoubleAnimation(0, 0, TimeSpan.FromSeconds(5));
            DoubleAnimation aniAngle = new DoubleAnimation(0, 45, TimeSpan.FromSeconds(5));

            //Empezar
            myTranslate.BeginAnimation(TranslateTransform.XProperty, aniX);
            myTranslate.BeginAnimation(TranslateTransform.YProperty, aniY);
            myRotateTransform.BeginAnimation(RotateTransform.AngleProperty, aniAngle);
        }
    }
}

