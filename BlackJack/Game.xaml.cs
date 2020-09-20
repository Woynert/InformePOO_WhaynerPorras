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

        List<Card> leftCards = new List<Card>();
        List<Card> rightCards = new List<Card>();

        int[,] rightPos = new int[9, 3]; //0X 1Y 3Angle
        int[,] leftPos = new int[9, 3]; //0X 1Y 3Angle



        public Game()
        {
            InitializeComponent();
            //iniciar
            setCardPos();
            dealer.Init();
        }
        
        private void setCardPos()
        { 
            
            //Asignar coordenadas
            Grid _grdSel = grdRightPos1;
            TransformGroup _rot = new TransformGroup();

            for(int l = 0; l < 9; l++)
            {
                switch(l){
                    case 0: _grdSel = grdRightPos1; break;
                    case 1: _grdSel = grdRightPos2; break;
                    case 2: _grdSel = grdRightPos3; break;
                    case 3: _grdSel = grdRightPos4; break;
                    case 4: _grdSel = grdRightPos5; break;
                    case 5: _grdSel = grdRightPos6; break;
                    case 6: _grdSel = grdRightPos7; break;
                    case 7: _grdSel = grdRightPos8; break;
                    case 8: _grdSel = grdRightPos9; break;
                }

                //Posición
                rightPos[l, 0] = Convert.ToInt32(_grdSel.Margin.Left);
                rightPos[l, 1] = Convert.ToInt32(_grdSel.Margin.Top);

                //Rotación
                _rot = (TransformGroup)_grdSel.RenderTransform;
                foreach (Transform t in _rot.Children)
                {
                    if (t is RotateTransform)
                    {
                        RotateTransform _r = (RotateTransform)t;
                        rightPos[l, 2] = Convert.ToInt32(_r.Angle);
                    }
                }
                _grdSel.Visibility = Visibility.Hidden;
                //MessageBox.Show("Current rotation angle : " + rightPos[l, 0].ToString());
            }
            
        }

        private void btnPedir_Click(object sender, RoutedEventArgs e) //Pedir
        {
            Card card = dealer.Deal(); 
            rightCards.Add(card);
            Grid _card = DrawCard(500, 300, card.Symbol, card.Suit);
            Animate(_card, 500, 500, 0);
            Animate(_card, rightPos[rightCards.Count - 1, 0], rightPos[rightCards.Count - 1, 1], 0);// rightPos[rightCards.Count - 1, 2]);
        }

        private void btnPlantar_Click(object sender, RoutedEventArgs e) //Plantarse
        {
            int cont = 0;
            foreach (Card card in dealer.deck)
            {
                DrawCard(150 * (cont - card.Suit * 13), 200 * card.Suit, card.Symbol, card.Suit);
                cont++;
            }
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

        private Grid DrawCard(int x, int y, int symbol, int suit)
        {
           
            int wt = 121;
            int ht = 189;
            y += -Convert.ToInt32(grdMaster.ActualHeight) + ht;
            x += -Convert.ToInt32(grdMaster.ActualWidth) +wt;

            //Color borde
            Color ColorStroke = new Color(); ColorStroke.A = 255; ColorStroke.R = 207; ColorStroke.G = ColorStroke.R; ColorStroke.B = ColorStroke.R;
            
            //Color Letra
            Color ColorValue;
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
            Thickness marginIcon = new Thickness(0, 20, 0, 0);
            imgNew.Margin = marginIcon;

            imgNew.Width = 89; imgNew.Height = imgNew.Width;
            imgNew.Source = btmIcon;// new BitmapImage(new Uri(@"Resources/Pica.png", UriKind.Relative));

            return (grdNew);
        }

        private void Animate(Grid obj, int x, int y, int angle)
        {
            int wt = Convert.ToInt32(obj.ActualWidth / 2);
            int ht = Convert.ToInt32(obj.ActualHeight / 2);

            //Crear transforms
            RotateTransform myRotateTransform = new RotateTransform();
            TranslateTransform myTranslate = new TranslateTransform();
            myTranslate.X = Convert.ToInt32(obj.Margin.Left);
            myTranslate.Y = Convert.ToInt32(obj.Margin.Top);
            ScaleTransform myScaleTransform = new ScaleTransform();
            myRotateTransform.CenterX = wt;
            myRotateTransform.CenterY = ht; 

            //Asignar
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(myTranslate);
            myTransformGroup.Children.Add(myScaleTransform);
            myTransformGroup.Children.Add(myRotateTransform);
            obj.RenderTransform = myTransformGroup;

            //Animar
            DoubleAnimation aniX = new DoubleAnimation(Convert.ToInt32(obj.Margin.Left), x, TimeSpan.FromSeconds(1));
            DoubleAnimation aniY = new DoubleAnimation(Convert.ToInt32(obj.Margin.Top), y, TimeSpan.FromSeconds(1));
            DoubleAnimation aniAngle = new DoubleAnimation(0, angle, TimeSpan.FromSeconds(0));
            DoubleAnimation aniCenterX = new DoubleAnimation(wt, x+wt, TimeSpan.FromSeconds(0));
            DoubleAnimation aniCenterY = new DoubleAnimation(ht, y+ht, TimeSpan.FromSeconds(0));

            //Empezar
            myTranslate.BeginAnimation(TranslateTransform.XProperty, aniX);
            myTranslate.BeginAnimation(TranslateTransform.YProperty, aniY);
            myRotateTransform.BeginAnimation(RotateTransform.AngleProperty, aniAngle);
            myRotateTransform.BeginAnimation(RotateTransform.CenterXProperty, aniCenterX);
            myRotateTransform.BeginAnimation(RotateTransform.CenterYProperty, aniCenterY);
        }
    }
}

