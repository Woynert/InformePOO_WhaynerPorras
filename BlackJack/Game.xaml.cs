using BlackJack.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;

namespace BlackJack
{
    /// <summary>
    /// Lógica de interacción para Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        public BitmapImage imgCorazon = new BitmapImage(new Uri(@"Resources/Corazon.png", UriKind.Relative));
        Dealer dealer = new Dealer();
        Player player = new Player();

        List<Card> leftCards = new List<Card>();
        List<Card> rightCards = new List<Card>();

        double[,] rightPos = new double[9, 3]; //0X 1Y 3Angle
        double[,] leftPos = new double[9, 3]; //0X 1Y 3Angle

        //SoundPlayer sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard1); //(@"Resources/GamblingCard1.wav");

        //Grid tester;
        DispatcherTimer Timer = new DispatcherTimer();
        int needrightCards = 0;
        int needLeftCards = 0;

        public Game()
        {
            InitializeComponent();
            //iniciar
            setCardPos();
            dealer.Init();

            Timer.Tick += dispatcherTimer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
        }
        
        private void setCardPos()
        { 
            
            //Asignar coordenadas
            Grid _grdSel = grdRightPos1;
            TransformGroup _rot = new TransformGroup();

            //Left
            for (int l = 0; l < 9; l++) 
            {
                switch (l)
                {
                    case 0: _grdSel = grdLeftPos1; break;
                    case 1: _grdSel = grdLeftPos2; break;
                    case 2: _grdSel = grdLeftPos3; break;
                    case 3: _grdSel = grdLeftPos4; break;
                    case 4: _grdSel = grdLeftPos5; break;
                    case 5: _grdSel = grdLeftPos6; break;
                    case 6: _grdSel = grdLeftPos7; break;
                    case 7: _grdSel = grdLeftPos8; break;
                    case 8: _grdSel = grdLeftPos9; break;
                }

                //Posición
                leftPos[l, 0] = _grdSel.Margin.Left;
                leftPos[l, 1] = _grdSel.Margin.Top;

                //Rotación
                _rot = (TransformGroup)_grdSel.RenderTransform;
                foreach (Transform t in _rot.Children)
                {
                    if (t is RotateTransform)
                    {
                        RotateTransform _r = (RotateTransform)t;
                        leftPos[l, 2] = Convert.ToInt32(_r.Angle);
                    }
                }
                _grdSel.Visibility = Visibility.Hidden;
                //MessageBox.Show("Current rotation angle : " + rightPos[l, 0].ToString());
            }

            //Right
            for (int l = 0; l < 9; l++) 
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
                rightPos[l, 0] = _grdSel.Margin.Left;
                rightPos[l, 1] = _grdSel.Margin.Top;

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

        private void btnStart_Click(object sender, RoutedEventArgs e) //Nueva Partida
        {
            needrightCards = 2;
            needLeftCards = 2;

            Timer.Start();
        }

        //Enviar Carta A La Izquierda
        private void SendCardLeft()
        {
            //get
            Card card = dealer.Deal();
            //add
            player.AddCard(card); 
            //draw
            Grid cardToDraw = DrawCard(Convert.ToInt32(grdMaster.ActualWidth / 4), -200, card.Symbol, card.Suit);
            //animate
            Animate(cardToDraw, leftPos[player.getCardCount() - 1, 0], leftPos[player.getCardCount() - 1, 1], leftPos[player.getCardCount() - 1, 2]);
            SoundCard();
            UpdateLabelScore();
        }
        //Enviar Carta A La Derecha
        private void SendCardRight()
        {
            //get
            Card card = dealer.Deal(); 
            //add
            dealer.AddCard(card); 
            //draw
            Grid cardToDraw = DrawCard(Convert.ToInt32(grdMaster.ActualWidth / 4), -200, card.Symbol, card.Suit); 
            //animate
            Animate(cardToDraw, rightPos[dealer.getCardCount() - 1, 0], rightPos[dealer.getCardCount() - 1, 1], rightPos[dealer.getCardCount() - 1, 2]);
            SoundCard();
            UpdateLabelScore();
        }

        private void btnPedir_Click(object sender, RoutedEventArgs e) //Pedir
        {
            SendCardLeft();
        }
        private void btnHaz_Click(object sender, RoutedEventArgs e) //Pedir un Haz
        {
            Card card = new Card(0, 1);
            //add
            player.AddCard(card);
            //draw
            Grid cardToDraw = DrawCard(Convert.ToInt32(grdMaster.ActualWidth / 4), -200, card.Symbol, card.Suit);
            //animate
            Animate(cardToDraw, leftPos[player.getCardCount() - 1, 0], leftPos[player.getCardCount() - 1, 1], leftPos[player.getCardCount() - 1, 2]);
            SoundCard();
            UpdateLabelScore();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) //Timer
        {
            if(needLeftCards > 0)
            {
                //Timer.Tick += dispatcherTimer_Tick;
                //Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
                //Timer.Start();
                SendCardLeft();
                needLeftCards--;
            }
            else if (needrightCards > 0)
            {
                SendCardRight();
                needrightCards--;
            }
            else
            {
                Timer.Stop();
            }
            
        }

        private void btnPlantar_Click(object sender, RoutedEventArgs e) //Plantarse
        {
            /*TransformGroup _rot = new TransformGroup();

            _rot = (TransformGroup)tester.RenderTransform;
            foreach (Transform t in _rot.Children)
            {
                if (t is TranslateTransform)
                {
                    TranslateTransform _r = (TranslateTransform)t;
                    MessageBox.Show(Convert.ToInt32(_r.X).ToString() + " : " + Convert.ToInt32(tester.Margin.Left).ToString() + " : " + Convert.ToInt32((tester.Margin.Left + Convert.ToInt32(grdMaster.ActualWidth) - 121)/2).ToString());
                }
            }
            Animate(tester, 0, 0, 180);*/

        }

        private int Check(List<Card> hand) //Sacar Valor
        {
            int val = 0;
            for(int i = 0; i < hand.Count; i++)
            {
                if (hand[i].Score == 1)
                {

                }
                val += hand[i].Score;
            }
            return (val);
        }

        private Grid DrawCard(int x, int y, int symbol, int suit)
        {
           
            int wt = 121;
            int ht = 189;
            x *= 2;
            y *= 2;
            x += -Convert.ToInt32(grdMaster.ActualWidth) +wt;
            y += -Convert.ToInt32(grdMaster.ActualHeight) + ht;
            

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

        private void Animate(Grid obj, double x, double y, double angle)
        {
            double wt = obj.ActualWidth / 2;
            double ht = obj.ActualHeight / 2;

            float segundos = 0.5f;

            //Crear transforms
            RotateTransform myRotateTransform = new RotateTransform();
            TranslateTransform myTranslate = new TranslateTransform();
            //myTranslate.X = Convert.ToInt32(obj.Margin.Left);
            //myTranslate.Y = Convert.ToInt32(obj.Margin.Top);
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
            double _x = x - (obj.Margin.Left + grdMaster.ActualWidth - 121)/2;
            double _y = y - (obj.Margin.Top + grdMaster.ActualHeight - 189) / 2;

            //MessageBox.Show(Convert.ToInt32(myTranslate.X).ToString() + " : " + Convert.ToInt32(obj.Margin.Left).ToString());
            DoubleAnimation aniX = new DoubleAnimation(0, _x, TimeSpan.FromSeconds(segundos));
            DoubleAnimation aniY = new DoubleAnimation(0, _y, TimeSpan.FromSeconds(segundos));
            DoubleAnimation aniAngle = new DoubleAnimation(0, angle, TimeSpan.FromSeconds(segundos));
            DoubleAnimation aniCenterX = new DoubleAnimation(0, _x + 121/2, TimeSpan.FromSeconds(segundos));
            DoubleAnimation aniCenterY = new DoubleAnimation(0, _y + 189/2, TimeSpan.FromSeconds(segundos));

            //Empezar
            myTranslate.BeginAnimation(TranslateTransform.XProperty, aniX);
            myTranslate.BeginAnimation(TranslateTransform.YProperty, aniY);
            myRotateTransform.BeginAnimation(RotateTransform.AngleProperty, aniAngle);
            myRotateTransform.BeginAnimation(RotateTransform.CenterXProperty, aniCenterX);
            myRotateTransform.BeginAnimation(RotateTransform.CenterYProperty, aniCenterY);
        }

        private void SoundCard()
        {
            Random _random = new Random();
            SoundPlayer sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard3);

            switch (_random.Next(0, 2))
            {
                case 2: sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard1); break;
                case 0: sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard2); break;
                case 1: sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard3); break;
            }

            sndPlayer.Load();
            sndPlayer.Play();
        }

        private void UpdateLabelScore()
        {
            lblScore.Content = Check(player.Hand).ToString();
        }

        private void lblScore_Initialized(object sender, EventArgs e) //Borde para Score
        {

            
            /*Color ColorValue = new Color(); ColorValue.A = 255; ColorValue.R = 0; ColorValue.G = 0; ColorValue.B = 0;
            int _border = 2;

            for(int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Label lblNew = new Label();
                    grdMaster.Children.Add(lblNew);

                    lblNew.FontFamily = new FontFamily("Source Code Pro");
                    lblNew.FontSize = lblScore.FontSize;
                    lblNew.Content = lblScore.Content;
                    lblNew.Foreground = new SolidColorBrush(ColorValue);

                    Thickness marginValue = new Thickness(lblScore.Margin.Left + i * _border, lblScore.Margin.Top + j * _border, 0, 0);
                    lblNew.Margin = marginValue;
                }
            }*/
            
            Canvas.SetZIndex(lblScore, 1);
        }
    }
}

