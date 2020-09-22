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

        List<Grid> gridLeftCards = new List<Grid>();
        List<Grid> gridRightCards = new List<Grid>();

        double[,] rightPos = new double[18, 3]; //0X 1Y 3Angle //Maximo 18 Cartas
        double[,] leftPos = new double[18, 3]; //0X 1Y 3Angle

        DispatcherTimer Timer = new DispatcherTimer();
        int timerAction = 0; //0Nada 1RepartirCartas 2MostrarCartasRight 3EntregarCartasDealer(ComprobarGanador) 4AnimaciónGanar 5AnimaciónPasado

        int needRightCards = 0;
        int needLeftCards = 0;

        readonly int cardW = 121;
        readonly int cardH = 189;

        //Animacion Ganar
        int RepeticionesTimer4 = 7;
        int RepeticionesTimer5 = 3;
        int RepeticionesTimer = 0;
        int ganador = 0; //0dealer 1player 2empate 3blackjack

        //Puntaje
        int partidasJugadas = 0;
        int partidasGanadas = 0;
        bool showDealerScore = false;

        public Game()
        {
            InitializeComponent();

            //iniciar
            setCardPos();
            dealer.Init();

            //Preparar animación
            Timer.Tick += dispatcherTimer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);

            //Bloquear control
            btnPlantar.Visibility = Visibility.Hidden;
            btnPedir.Visibility = Visibility.Hidden;
            btnHaz.Visibility = Visibility.Hidden;
        }
        
        private void setCardPos()
        { 
            
            //Asignar coordenadas
            Grid _grdSel = grdRightPos1;
            TransformGroup _rot = new TransformGroup();

            //Left
            for (int l = 0; l < 18; l++) 
            {
                switch (l)
                {
                    case 0: case 9: _grdSel = grdLeftPos1; break;
                    case 1: case 10: _grdSel = grdLeftPos2; break;
                    case 2: case 11: _grdSel = grdLeftPos3; break;
                    case 3: case 12: _grdSel = grdLeftPos4; break;
                    case 4: case 13: _grdSel = grdLeftPos5; break;
                    case 5: case 14: _grdSel = grdLeftPos6; break;
                    case 6: case 15: _grdSel = grdLeftPos7; break;
                    case 7: case 16: _grdSel = grdLeftPos8; break;
                    case 8: case 17: _grdSel = grdLeftPos9; break;
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
            //eliminar cartas
            for (int j = 0; j < gridLeftCards.Count; j++)
            {
                grdMaster.Children.Remove(gridLeftCards[j]);
            }
            for (int j = 0; j < gridRightCards.Count; j++)
            {
                grdMaster.Children.Remove(gridRightCards[j]);
            }

            //limpiar y empezar de nuevo
            dealer.Generate();
            dealer.Randomize();
            gridLeftCards = new List<Grid>();
            player.Hand = new List<Card>();
            gridRightCards = new List<Grid>();
            dealer.Hand = new List<Card>();

            RepeticionesTimer = 0;
            lblWinner.Content = "";

            //Change label color (AMARILLO)
            Color color = new Color(); color.A = 20 * 250 / 100; color.R = 0; color.G = 0; color.B = 0;
            Color color2 = new Color(); color2.A = 255; color2.R = 255; color2.G = 255; color2.B = 0;
            rctScorePlayer.Fill = new SolidColorBrush(color);
            rctScoreDealer.Fill = new SolidColorBrush(color);
            lblScorePlayer.Foreground = new SolidColorBrush(color2);
            lblScoreDealer.Content = "?";
            showDealerScore = false;

            //Bloquear control
            btnPlantar.Visibility = Visibility.Hidden; 
            btnPedir.Visibility = Visibility.Hidden;
            btnStart.Visibility = Visibility.Hidden;

            //repartir
            needRightCards = 2;
            needLeftCards = 2;
            timerAction = 1;
            Timer.Start();
            btnHolder.Focus();
        }

        //Enviar Carta A La Izquierda
        private void SendCardLeft(bool mA = false)
        {
            //get
            Card card;
            if (!mA) card = dealer.Deal();
            else card = new Card(new Random().Next(0, 4), 1);
            //add
            player.AddCard(card); 
            //draw
            Grid cardToDraw = DrawCard(Convert.ToInt32(grdMaster.ActualWidth / 4), -200, card.Symbol, card.Suit, false);
            //add to gridlist
            gridLeftCards.Add(cardToDraw);
            //animate
            Animate(cardToDraw, leftPos[player.getCardCount() - 1, 0], leftPos[player.getCardCount() - 1, 1], leftPos[player.getCardCount() - 1, 2], Convert.ToInt32(grdMaster.ActualWidth / 4), -200);
            //sound
            SoundCard(0);
            UpdateLabelScore();

            if ((Check(player.Hand) == 21) && (player.getCardCount() == 2))
            {
                Ganador(3);

                //Bloquear control
                btnPlantar.Visibility = Visibility.Hidden;
                btnPedir.Visibility = Visibility.Hidden;
                btnHaz.Visibility = Visibility.Hidden;

                //detener reparto
                needRightCards = 0;
                needLeftCards = 0;
            }
            else if (Check(player.Hand) >= 21) //Ya no puede recibir más cartas
            {

                //Bloquear control
                btnPlantar.Visibility = Visibility.Hidden;
                btnPedir.Visibility = Visibility.Hidden;
                btnHaz.Visibility = Visibility.Hidden;

                //Change rect color (ROJO)
                if (Check(player.Hand) > 21)
                {
                    Color color = new Color(); color.A = 100; color.R = 255; color.G = 0; color.B = 0; //AMARILLO Color color = new Color(); color.A = 255; color.R = 255; color.G = 255; color.B = 0;
                    Color color2 = new Color(); color2.A = 255; color2.R = 255; color2.G = 255; color2.B = 255;
                    rctScorePlayer.Fill = new SolidColorBrush(color);
                    lblScorePlayer.Foreground = new SolidColorBrush(color2);
                }

                timerAction = 5;
                Timer.Start();
            }
        }

        //Enviar Carta A La Derecha
        private void SendCardRight(bool back)
        {
            //get
            Card card = dealer.Deal(); 
            card.Back = back;
            //add
            dealer.AddCard(card); 
            //draw
            Grid cardToDraw = DrawCard(Convert.ToInt32(grdMaster.ActualWidth / 4), -200, card.Symbol, card.Suit, card.Back);
            //add to gridlist
            gridRightCards.Add(cardToDraw);
            //animate
            Animate(cardToDraw, rightPos[dealer.getCardCount() - 1, 0], rightPos[dealer.getCardCount() - 1, 1], rightPos[dealer.getCardCount() - 1, 2], Convert.ToInt32(grdMaster.ActualWidth / 4), -200);
            //sound
            SoundCard(0);
            UpdateLabelScore();

            //Change rect color (ROJO)
            if (Check(dealer.Hand) > 21)
            {
                Color color = new Color(); color.A = 100; color.R = 255; color.G = 0; color.B = 0;
                rctScoreDealer.Fill = new SolidColorBrush(color);
            }
        }

        private void btnPedir_Click(object sender, RoutedEventArgs e) //Pedir
        {
            SendCardLeft();
            btnHolder.Focus();
        }

        private void btnPlantar_Click(object sender, RoutedEventArgs e) //Plantarse
        {
            //Bloquear control
            btnPlantar.Visibility = Visibility.Hidden;
            btnPedir.Visibility = Visibility.Hidden;
            btnHaz.Visibility = Visibility.Hidden;

            timerAction = 2;
            Timer.Start();
            btnHolder.Focus();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) //Timer
        {
            //MessageBox.Show(RepeticionesTimer.ToString() + " : " + timerAction.ToString());
            switch (timerAction)
            {
                case 1: //Repartir Cartas
                    if (needLeftCards > 0)
                    {
                        SendCardLeft();
                        needLeftCards--;
                    }
                    else if (needRightCards > 0)
                    {
                        if (dealer.getCardCount() > 0) {
                            SendCardRight(true);
                        }
                        else SendCardRight(false);
                        needRightCards--;
                    }
                    else
                    {
                        //Desbloquear control
                        btnPlantar.Visibility = Visibility.Visible;
                        btnPedir.Visibility = Visibility.Visible;
                        btnHaz.Visibility = Visibility.Visible;

                        timerAction = 0; //reset
                        Timer.Stop();
                    }
                    break;

                case 2: //Voltear Cartas
                    showDealerScore = true;
                    bool next = false;
                    for (int i = 0; i < dealer.getCardCount(); i++)
                    {
                        if (dealer.Hand[i].Back)
                        {
                            next = true;
                            dealer.Hand[i].Back = false;
                            ChangeSide(gridRightCards[i], dealer.Hand[i].Back);
                            UpdateLabelScore();
                            break;
                        }
                    }
                    if (!next)
                    {
                        timerAction = 3; //repartir cartas a dealer (postgame)
                    }
                    break;
                case 3:
                    if (Check(player.Hand) > 21) //Player pasado
                    {
                        timerAction = 0; //reset
                        Timer.Stop(); 
                        Ganador(0);
                    }
                    else if (Check(player.Hand) == Check(dealer.Hand) && (Check(player.Hand) == 21)) //Doble BlackJack!!!
                    {
                        Ganador(2);
                    }
                    else if (Check(dealer.Hand) <= Check(player.Hand) && Check(dealer.Hand) < 21) //Dealer todavia puede recibir
                    {
                        SendCardRight(false);
                    }
                    else if (Check(dealer.Hand) > 21) //Dealer pasado
                    {
                        timerAction = 0; //reset
                        Timer.Stop(); 
                        Ganador(1);
                    }
                    else if (Check(dealer.Hand) <= 21 && Check(dealer.Hand) > Check(player.Hand)) //Dealer Gana
                    {
                        timerAction = 0; //reset
                        Timer.Stop(); 
                        Ganador(0);
                    }
                    break;
                case 4: //AnimaciónGanador
                    //MessageBox.Show(RepeticionesTimer.ToString());
                    if (RepeticionesTimer < RepeticionesTimer4)
                    {
                        RepeticionesTimer++;
                    }
                    else
                    {
                        //Desbloquear Start
                        btnStart.Visibility = Visibility.Visible;

                        //Final de la Animación
                        RepeticionesTimer = 0;
                        RemoveAllCards(ganador);
                        Timer.Stop();
                    }
                    break;
                case 5: //AmaciónPasado
                    if (RepeticionesTimer < RepeticionesTimer5)
                    {
                        RepeticionesTimer++;
                    }
                    else
                    {

                        //Final de la Animación
                        RepeticionesTimer = 0;
                        timerAction = 2; 
                        Timer.Start();
                    }
                    break;
            }
        }

        private int Check(List<Card> hand) //Sacar Valor
        {
            int val = 0;
            int HazCount = 0;
            for(int i = 0; i < hand.Count; i++)
            {
                if (hand[i].Score == 1) //Haz detectado
                {
                    HazCount++;
                    /*if (val + 11 <= 21)
                    {
                        val += 11;
                    }
                    else val += 1;*/
                }
                else val += hand[i].Score;
            }
            for (int i = 1; i <= HazCount; i++)
            {
                if (val + 11 <= 21)
                {
                    val += 11;
                }
                else val += 1;
            }
            return (val);
        }

        private Grid DrawCard(int x, int y, int symbol, int suit, bool back)
        {
           
            int wt = cardW;
            int ht = cardH;
            x *= 2;
            y *= 2;
            x += -Convert.ToInt32(grdMaster.ActualWidth) +wt;
            y += -Convert.ToInt32(grdMaster.ActualHeight) + ht;

            //Color borde
            Color ColorStroke = new Color(); ColorStroke.A = 255; ColorStroke.R = 180; ColorStroke.G = ColorStroke.R; ColorStroke.B = ColorStroke.R;
            
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
            Grid      grdNew  = new Grid();
            Rectangle rctNew  = new Rectangle();
            Label     lblNew  = new Label();
            Image     imgNew  = new Image();
            Image     img2New = new Image();

            grdMaster.Children.Add(grdNew);

            //Grid Base
            Thickness marginGrid = new Thickness(x, y, 0, 0);
            grdNew.Margin = marginGrid;
            grdNew.Width = wt;
            grdNew.Height = ht;
            grdNew.Children.Add(rctNew);
            grdNew.Children.Add(lblNew);
            grdNew.Children.Add(imgNew);
            grdNew.Children.Add(img2New);

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
            imgNew.Source = btmIcon; //new BitmapImage(new Uri(@"Resources/Pica.png", UriKind.Relative));

            //MiniIcon
            img2New.Width = 30; img2New.Height = img2New.Width;

            Thickness marginIcon2 = new Thickness(cardW/2 + img2New.Width/3, -cardH +img2New.Width*2 +8, 0, 0);
            img2New.Margin = marginIcon2;
            img2New.Source = btmIcon;
            ChangeSide(grdNew, back);
            return (grdNew);
        }

        private void ChangeSide(Grid obj, bool back)
        {
            SoundCard(0); //sonido
            if (back) { //Mostrar Lado Trasero
                foreach (UIElement elm in obj.Children)
                {
                    if (elm is Rectangle)
                    {
                        //Color borde
                        Rectangle rec = (Rectangle)elm;

                        Color ColorStroke = new Color(); ColorStroke.A = 255; ColorStroke.R = 250; ColorStroke.G = ColorStroke.R; ColorStroke.B = ColorStroke.R;
                        rec.Stroke = new SolidColorBrush(ColorStroke);

                        rec.Fill = new ImageBrush { ImageSource = imgImagePlace2.Source };
                    }
                    if (elm is Image)
                    {
                        elm.Visibility = Visibility.Hidden;
                    }
                
                    if (elm is Label)
                    {
                        elm.Visibility = Visibility.Hidden;
                    }
                }
            }
            else //Mostrar Frente
            {
                foreach (UIElement elm in obj.Children)
                {
                    if (elm is Rectangle)
                    {
                        //Color borde
                        Rectangle rec = (Rectangle)elm;

                        Color ColorStroke = new Color(); ColorStroke.A = 255; ColorStroke.R = 180; ColorStroke.G = ColorStroke.R; ColorStroke.B = ColorStroke.R;
                        rec.Stroke = new SolidColorBrush(ColorStroke);

                        rec.Fill = new ImageBrush { ImageSource = imgImagePlace.Source };
                    }
                    if (elm is Image)
                    {
                        elm.Visibility = Visibility.Visible;
                    }

                    if (elm is Label)
                    {
                        elm.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void Animate(Grid obj, double x, double y, double angle, double xStart = 0, double yStart = 0, double angleStart = 0)
        {
            double wt = obj.ActualWidth / 2;
            double ht = obj.ActualHeight / 2;

            float segundos = 0.5f;

            double _xStart = xStart - (obj.Margin.Left + grdMaster.ActualWidth - cardW) / 2;
            double _yStart = yStart - (obj.Margin.Top + grdMaster.ActualHeight - cardH) / 2;

            //Crear transforms
            RotateTransform myRotateTransform = new RotateTransform();
            TranslateTransform myTranslate = new TranslateTransform();
            myTranslate.X = _xStart; //Convert.ToInt32(obj.Margin.Left);
            myTranslate.Y = _yStart; //Convert.ToInt32(obj.Margin.Top);
            ScaleTransform myScaleTransform = new ScaleTransform();
            myRotateTransform.Angle = angleStart;
            myRotateTransform.CenterX = wt;
            myRotateTransform.CenterY = ht; 

            //Asignar
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(myTranslate);
            myTransformGroup.Children.Add(myScaleTransform);
            myTransformGroup.Children.Add(myRotateTransform);
            obj.RenderTransform = myTransformGroup;

            //Animar
            double _x = x - (obj.Margin.Left + grdMaster.ActualWidth - cardW) / 2;
            double _y = y - (obj.Margin.Top + grdMaster.ActualHeight - cardH) / 2;

            //MessageBox.Show(Convert.ToInt32(myTranslate.X).ToString() + " : " + Convert.ToInt32(obj.Margin.Left).ToString());
            DoubleAnimation aniX = new DoubleAnimation(_xStart, _x, TimeSpan.FromSeconds(segundos));
            DoubleAnimation aniY = new DoubleAnimation(_yStart, _y, TimeSpan.FromSeconds(segundos));
            DoubleAnimation aniAngle = new DoubleAnimation(angleStart, angle, TimeSpan.FromSeconds(segundos));
            DoubleAnimation aniCenterX = new DoubleAnimation(_xStart, _x + cardW / 2, TimeSpan.FromSeconds(segundos));
            DoubleAnimation aniCenterY = new DoubleAnimation(_yStart, _y + cardH / 2, TimeSpan.FromSeconds(segundos));

            //Empezar
            myTranslate.BeginAnimation(TranslateTransform.XProperty, aniX);
            myTranslate.BeginAnimation(TranslateTransform.YProperty, aniY);
            myRotateTransform.BeginAnimation(RotateTransform.AngleProperty, aniAngle);
            myRotateTransform.BeginAnimation(RotateTransform.CenterXProperty, aniCenterX);
            myRotateTransform.BeginAnimation(RotateTransform.CenterYProperty, aniCenterY);
        }

        private void SoundCard(int ind)
        {
            SoundPlayer sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard3);
            switch (ind)
            {
                case 0: //Card
                    Random _random = new Random();
                    switch (_random.Next(0, 2))
                    {
                        case 2: sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard1); break;
                        case 0: sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard2); break;
                        case 1: sndPlayer = new SoundPlayer(Properties.Resources.GamblingCard3); break;
                    }
                    break;
                case 1: //Right
                    sndPlayer = new SoundPlayer(Properties.Resources.Right);
                    break;
                case 2: //Wrong
                    sndPlayer = new SoundPlayer(Properties.Resources.Wrong);
                    break;
            }
            sndPlayer.Load();
            sndPlayer.Play();
        }

        private void UpdateLabelScore()
        {
            lblScorePlayer.Content = Check(player.Hand).ToString();
            if (showDealerScore) lblScoreDealer.Content = Check(dealer.Hand).ToString();
            Canvas.SetZIndex(grdUI, 1);
        }
        private void UpdatePartidasPuntuacion()
        {
            lblJugadas.Content = partidasJugadas.ToString();
            lblGanadas.Content = partidasGanadas.ToString();
        }

        private void RemoveAllCards(int ganador)
        {

            int i;
            if (ganador == 1 || ganador == 3) //true Player/BlackJack
            {
                i = 0;
                foreach (Grid obj in gridLeftCards)
                {
                    Animate(obj, grdMaster.ActualWidth / 2, grdMaster.ActualHeight, 0, leftPos[i, 0], leftPos[i, 1], leftPos[i, 2]);
                    i++;
                }
                i = 0;
                foreach (Grid obj in gridRightCards)
                {
                    Animate(obj, grdMaster.ActualWidth / 2, grdMaster.ActualHeight, 0, rightPos[i, 0], rightPos[i, 1], rightPos[i, 2]);
                    i++;
                }
            }
            else //false Dealer/Empate
            {
                i = 0;
                foreach (Grid obj in gridLeftCards)
                {
                    Animate(obj, grdMaster.ActualWidth / 2, -cardH*2, 0, leftPos[i, 0], leftPos[i, 1], leftPos[i, 2]);
                    i++;
                }
                i = 0;
                foreach (Grid obj in gridRightCards)
                {
                    Animate(obj, grdMaster.ActualWidth / 2, -cardH*2, 0, rightPos[i, 0], rightPos[i, 1], rightPos[i, 2]);
                    i++;
                }
            }
        }

        private void Ganador(int ganador)
        {
            this.ganador = ganador;
            switch (ganador)
            {
                case 0: //Dealer
                    lblWinner.Content = "Perdedor";
                    SoundCard(2);
                    break;
                case 1: //Player
                    lblWinner.Content = "Ganador";
                    partidasGanadas++;
                    SoundCard(1);
                    break;
                case 2: //empate
                    lblWinner.Content = "Empate";
                    SoundCard(2);
                    break;
                case 3: //blackjack
                    lblWinner.Content = "BLACKJACK";
                    partidasGanadas++;
                    SoundCard(1);
                    break;
            }
            partidasJugadas++;
            UpdatePartidasPuntuacion();

            //Inicio de la animación
            timerAction = 4;
            Timer.Start();
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
            
            
        }

        private void btnHaz_Click(object sender, RoutedEventArgs e) //Secreto
        {
            SendCardLeft(true); 
        }
    }
}

