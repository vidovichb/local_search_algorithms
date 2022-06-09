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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lokalni_iskalni_algoritmi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Checkerboard(int[] positions)
        {
            
            SolidColorBrush defaultBrush = new SolidColorBrush(Colors.White);
            SolidColorBrush alternateBrush = new SolidColorBrush(Colors.Gray);

            bool alternate = true;
            int isodd = positions.Length % 2;

            for (int i = 0; i < positions.Length; i++)
            {
                for (int j = 0; j < positions.Length; j++)
                {
                    string cellid = "id" + i + j;

                    var imgBrush = new ImageBrush();
                    imgBrush.ImageSource = new BitmapImage(new Uri(@"chessqueen.png", UriKind.Relative));

                    Grid cell = new Grid();
                    Grid gg = new Grid();

                    if (alternate)
                    {
                        cell.Background = defaultBrush;

                        gg.Name = cellid;
                        gg.Background = imgBrush;
                        if (positions[j] == i)
                        {
                            gg.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            gg.Visibility = Visibility.Collapsed;
                        }
                        cell.Children.Add(gg);

                        ChessBoard.Children.Add(cell);

                        alternate = false;
                        if (j == (positions.Length - 1) && isodd == 0)
                        {
                            alternate = true;
                        }
                    }
                    else
                    {
                        cell.Background = alternateBrush;

                        gg.Name = cellid;
                        gg.Background = imgBrush;
                        if (positions[j] == i)
                        {
                            gg.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            gg.Visibility = Visibility.Collapsed;
                        }
                        cell.Children.Add(gg);

                        ChessBoard.Children.Add(cell);

                        alternate = true;
                        if (j == (positions.Length - 1) && isodd == 0)
                        {
                            alternate = false;
                        }
                    }

                }
            }
            
        }

        private int[] GenerateQueenPos(int queens)
        {
            int[] positions = new int[queens];
            for (int i = 0; i < queens; i++)
            {
                int qline = rnd.Next(queens);
                positions[i] = qline;
            }
            return positions;
        }

        private int IzrHevristike(int[] QueenPos)
        {
            
            int hevr = 0;
            
            for (int i = 0; i < QueenPos.Length; i++)
            {
                int updiag = -1;
                int downdiag = 1;
                for (int j = i + 1; j < QueenPos.Length; j++)
                {
                    if (QueenPos[i] == QueenPos[j] || QueenPos[i] == (QueenPos[j] + updiag) || QueenPos[i] == (QueenPos[j] + downdiag))
                    {
                        hevr++;
                    }
                    updiag--;
                    downdiag++;
                }
            }
            return hevr;
        }

       

        private int[] hillClimb(int pomiki,int[] startingpos)
        {
            int[] prevPosit = startingpos;
            int mov = pomiki;
            int koraki = 0;
            while (mov >0) {
                int prevHevr = IzrHevristike(prevPosit);
                StkorakovUI.Content = koraki;
                if (prevHevr==0) {
                    break;
                }
                List<List<int>> avail = new List<List<int>>();
                int savepos = 0;
                for (int i = 0; i < prevPosit.Length; i++) {
                    int[] temp = prevPosit;
                    int tmp = prevPosit[i];
                    for (int j = 0; j < prevPosit.Length; j++) {
                        if (j == 0) {
                            savepos = temp[i];
                        }
                        if (tmp != j)
                        {
                            temp[i] = j;
                            int Hevr = IzrHevristike(temp);
                            if (Hevr == prevHevr) {
                                List<int> outtemp = new List<int>();
                                for (int c = 0; c < temp.Length; c++)
                                {
                                    outtemp.Add(temp[c]);
                                }
                                avail.Add(outtemp);
                            }
                            if (Hevr < prevHevr)
                            {
                                avail.RemoveRange(0, avail.Count);
                                List<int> outtemp = new List<int>();
                                for (int c = 0; c < temp.Length; c++)
                                {
                                    outtemp.Add(temp[c]);
                                }
                                avail.Add(outtemp);
                            }
                        }
                    }
                    temp[i] = savepos;
                }
                if (avail.Count>1) {
                    mov--;
                }
                if (avail.Count>0) {
                    int index = rnd.Next(avail.Count);

                    prevPosit = avail[index].ToArray();
                }
                koraki++;
            }
            return prevPosit;
        }

        private int[] simAnneal(int starttmp, int sprtmp, int[] startingpos)
        {
            int[] prevPosit = startingpos;
            int begintmp = starttmp;
            int changetmp = sprtmp;

            int koraki = 1;
            while (begintmp > 0)
            {
                int[] savedpos = prevPosit;
                int prevHevr = IzrHevristike(prevPosit);
                StkorakovUI.Content = koraki;
                if (prevHevr == 0)
                {
                    break;
                }

                List<List<int>> avail = new List<List<int>>();
                int savepos = 0;
                for (int i = 0; i < prevPosit.Length; i++)
                {
                    int[] temp = prevPosit;
                    int tmp = prevPosit[i];
                    for (int j = 0; j < prevPosit.Length; j++)
                    {
                        if (j == 0)
                        {
                            savepos = temp[i];
                        }
                        if (tmp != j)
                        {
                            temp[i] = j;

                            List<int> outtemp = new List<int>();
                            for (int c = 0; c < temp.Length; c++)
                            {
                                outtemp.Add(temp[c]);
                            }
                            avail.Add(outtemp);
                        }

                    }
                    temp[i] = savepos;
                }
                //IZBEREM NAKLJUCNEGA
                int index = rnd.Next(avail.Count);
                prevPosit = avail[index].ToArray();


                int newHevr = IzrHevristike(prevPosit);
                double hevrDelta = prevHevr - newHevr;
                if (hevrDelta < 0)
                {
                    double rescc = Math.Pow(Math.E, ((hevrDelta) / begintmp));

                    double movetolesser = rnd.NextDouble();
                    if (movetolesser > rescc)
                    {
                        prevPosit = savedpos;
                    }
                }
                koraki++;
                begintmp -= changetmp;
            }
            return prevPosit;
        }

        private int[] LocalBeamS(int stanja, int iteracije,int[] startingposition)
        {
            int iter = iteracije;
            List<int[]> zacetnastanja = new List<int[]>();
            int koraki = 0;
            for (int i = 0; i < stanja; i++)
            {
                int[] positions = GenerateQueenPos(currentboardsize);
                zacetnastanja.Add(positions);
            }

            int[] prevPosit = new int[currentboardsize];
            while (iter>0) {

                zacetnastanja.Sort((a, b) => (IzrHevristike(a).CompareTo(IzrHevristike(b))));

                int testHevr = IzrHevristike(zacetnastanja[0]);
                StkorakovUI.Content = koraki;
                if (testHevr == 0) {
                    break;
                }
                //Razvijem stanja
                List<int[]> avail = new List<int[]>();

                for (int p = 0; p < stanja; p++) {
                    prevPosit = zacetnastanja[p];
                    int savepos = 0;
                    for (int i = 0; i < prevPosit.Length; i++)
                    {
                        int[] temp = prevPosit;
                        int tmp = prevPosit[i];
                        for (int j = 0; j < prevPosit.Length; j++)
                        {
                            if (j == 0)
                            {
                                savepos = temp[i];
                            }
                            if (tmp != j)
                            {
                                temp[i] = j;

                                List<int> outtemp = new List<int>();
                                for (int c = 0; c < temp.Length; c++)
                                {
                                    outtemp.Add(temp[c]);
                                }
                                avail.Add(outtemp.ToArray());
                            }
                        }
                        temp[i] = savepos;
                    }
                }
                zacetnastanja.RemoveRange(0,zacetnastanja.Count);
                zacetnastanja = avail;
                koraki++;
                iter--;
            }
            return zacetnastanja[0];
        }

        private int[] GenAlg(int populat,int elit, int krizan, int mutat, int gen)
        {
            int stejGen = 0;
            List<int[]> zacetnastanja = new List<int[]>();

            for (int i = 0; i < populat; i++)
            {
                int[] positions = GenerateQueenPos(currentboardsize);
                zacetnastanja.Add(positions);
            }

            int[] tempPosit = new int[currentboardsize];

            while (stejGen < gen) {
                zacetnastanja.Sort((a, b) => (IzrHevristike(a).CompareTo(IzrHevristike(b))));
                zacetnastanja.RemoveRange(populat,zacetnastanja.Count-populat);
                int testHevr = IzrHevristike(zacetnastanja[0]);
                StkorakovUI.Content = stejGen+1;
                if (testHevr == 0)
                {
                    tempPosit = zacetnastanja[0];
                    break;
                }
                int[] parent1 = new int[currentboardsize];
                int[] parent2 = new int[currentboardsize];
                int[] parent11 = new int[currentboardsize];
                int[] parent22 = new int[currentboardsize];
                int[] child1 = new int[currentboardsize];
                int[] child2 = new int[currentboardsize];
                int[] child11 = new int[currentboardsize];
                int[] child22 = new int[currentboardsize];

                for (int i=0;i<populat/2;i++) {
                    int operated = 0;
                    parent1 = zacetnastanja[rnd.Next(0,populat)];
                    parent2 = zacetnastanja[rnd.Next(0,populat)];
                    parent11 = zacetnastanja[rnd.Next(0, populat)];
                    parent22 = zacetnastanja[rnd.Next(0, populat)];
                    
                    if (rnd.Next(100) < krizan) {
                        operated=1;
                        int crosspoint = rnd.Next(currentboardsize);
                        int opt = rnd.Next(1);
                        if (opt == 0) {
                            for (int j = 0; j < parent1.Length; j++) {
                                int tem1 = parent1[j];
                                int tem2 = parent2[j];
                                if (j < crosspoint)
                                {
                                    child1[j] = tem1;
                                    child2[j] = tem2;
                                }
                                else {
                                    child1[j] = tem2;
                                    child2[j] = tem1;
                                }
                            }
                        }
                        else {
                            for (int j = 0; j < parent1.Length; j++)
                            {
                                int tem1 = parent1[j];
                                int tem2 = parent2[j];
                                if (j < crosspoint)
                                {
                                    child1[parent1.Length - 1 - j] = tem1;
                                    child2[parent1.Length - 1 - j] = tem2;
                                }
                                else
                                {
                                    child1[parent1.Length - 1 - j] = tem2;
                                    child2[parent1.Length - 1 - j] = tem1;
                                }
                            }
                        }

                    }

                    if (operated==1)
                    {
                        zacetnastanja.Add(child1);
                        zacetnastanja.Add(child2);
                    }

                    if (rnd.Next(100) < mutat)
                    {
                        operated=2;

                        for (int j = 0; j < parent11.Length; j++)
                        {
                            int tem1 = parent11[j];
                            int tem2 = parent22[j];

                            int mutinx = rnd.Next(parent11.Length);

                            child11[j] = tem1;
                            child22[j] = tem2;
                            if (j == mutinx) {
                                int chpos = rnd.Next(parent11.Length);
                                while (tem1 == chpos) {
                                    chpos = rnd.Next(parent11.Length);
                                }
                                child11[j] = chpos;
                                while (tem2 == chpos)
                                {
                                    chpos = rnd.Next(parent11.Length);
                                }
                                child22[j] = chpos;
                            }
                        }
                    }
                    if (operated==2) {
                        zacetnastanja.Add(child11);
                        zacetnastanja.Add(child22);
                    }
                }
                stejGen++;
            }
            return zacetnastanja[0];
        }

        private void Button_Click(object sender, RoutedEventArgs e) //zazeni algoritem btn
        {
            if (Hillclm.IsChecked == true)
            {
                string sameHmove = EnakPomik.Text;
                int HillMoves = Convert.ToInt32(sameHmove);

                int[] newPositions = hillClimb(HillMoves, startingposition);
                ChessBoard.Children.RemoveRange(0, currentboardsize * currentboardsize);
                Checkerboard(newPositions);
                currentboardsize = newPositions.Length;
                startingposition = newPositions;
                int hh = IzrHevristike(newPositions);
                IzpHevr.Content = hh;
            }
            else if (Simanneal.IsChecked == true)
            {
                string stTmp = ZacTmp.Text;
                string sprtmpstr = SprTemp.Text;
                int begintmp = Convert.ToInt32(stTmp);
                int changtmp = Convert.ToInt32(sprtmpstr);

                int[] newPositions = simAnneal(begintmp, changtmp, startingposition);
                int hh = IzrHevristike(newPositions);
                ChessBoard.Children.RemoveRange(0, currentboardsize * currentboardsize);
                Checkerboard(newPositions);
                currentboardsize = newPositions.Length;
                startingposition = newPositions;
                IzpHevr.Content = hh;

            }
            else if (LBS.IsChecked == true)
            {
                string ststanj = StStanj.Text;
                string stiteracij = StIterac.Text;
                int stanja = Convert.ToInt32(ststanj);
                int iteracije = Convert.ToInt32(stiteracij);

                int[] newPositions = LocalBeamS(stanja, iteracije, startingposition);
                int hh = IzrHevristike(newPositions);
                ChessBoard.Children.RemoveRange(0, currentboardsize * currentboardsize);
                Checkerboard(newPositions);
                currentboardsize = newPositions.Length;
                startingposition = newPositions;
                IzpHevr.Content = hh;
            }
            else if (Genal.IsChecked == true)
            {
                int Populacija = Convert.ToInt32(VelPop.Text);
                int Elit = Convert.ToInt32(Elita.Text);
                int kriz = Convert.ToInt32(Krizanje.Text);
                int mutac = Convert.ToInt32(Mutacije.Text);
                int gen = Convert.ToInt32(Generacije.Text);

                int[] newPositions = GenAlg(Populacija,Elit,kriz,mutac,gen);
                int hh = IzrHevristike(newPositions);
                ChessBoard.Children.RemoveRange(0, currentboardsize * currentboardsize);
                Checkerboard(newPositions);
                currentboardsize = newPositions.Length;
                startingposition = newPositions;
                IzpHevr.Content = hh;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Hillclm_Checked(object sender, RoutedEventArgs e)
        {
            HillclmLBL.Visibility = Visibility.Visible;
            EnakPomik.Visibility = Visibility.Visible;

            SimannealLBL1.Visibility = Visibility.Collapsed;
            ZacTmp.Visibility = Visibility.Collapsed;
            SimannealLBL2.Visibility = Visibility.Collapsed;
            SprTemp.Visibility = Visibility.Collapsed;
            LBSLBL1.Visibility = Visibility.Collapsed;
            StStanj.Visibility = Visibility.Collapsed;
            LBSLBL2.Visibility = Visibility.Collapsed;
            StIterac.Visibility = Visibility.Collapsed;
            GenalLBL1.Visibility = Visibility.Collapsed;
            VelPop.Visibility = Visibility.Collapsed;
            GenalLBL2.Visibility = Visibility.Collapsed;
            Elita.Visibility = Visibility.Collapsed;
            GenalLBL3.Visibility = Visibility.Collapsed;
            Krizanje.Visibility = Visibility.Collapsed;
            GenalLBL4.Visibility = Visibility.Collapsed;
            Mutacije.Visibility = Visibility.Collapsed;
            GenalLBL5.Visibility = Visibility.Collapsed;
            Generacije.Visibility = Visibility.Collapsed;
        }

        private void Simanneal_Checked(object sender, RoutedEventArgs e)
        {
            SimannealLBL1.Visibility = Visibility.Visible;
            ZacTmp.Visibility = Visibility.Visible;
            SimannealLBL2.Visibility = Visibility.Visible;
            SprTemp.Visibility = Visibility.Visible;

            HillclmLBL.Visibility = Visibility.Collapsed;
            EnakPomik.Visibility = Visibility.Collapsed;
            LBSLBL1.Visibility = Visibility.Collapsed;
            StStanj.Visibility = Visibility.Collapsed;
            LBSLBL2.Visibility = Visibility.Collapsed;
            StIterac.Visibility = Visibility.Collapsed;
            GenalLBL1.Visibility = Visibility.Collapsed;
            VelPop.Visibility = Visibility.Collapsed;
            GenalLBL2.Visibility = Visibility.Collapsed;
            Elita.Visibility = Visibility.Collapsed;
            GenalLBL3.Visibility = Visibility.Collapsed;
            Krizanje.Visibility = Visibility.Collapsed;
            GenalLBL4.Visibility = Visibility.Collapsed;
            Mutacije.Visibility = Visibility.Collapsed;
            GenalLBL5.Visibility = Visibility.Collapsed;
            Generacije.Visibility = Visibility.Collapsed;
        }

        private void LBS_Checked(object sender, RoutedEventArgs e)
        {
            LBSLBL1.Visibility = Visibility.Visible;
            StStanj.Visibility = Visibility.Visible;
            LBSLBL2.Visibility = Visibility.Visible;
            StIterac.Visibility = Visibility.Visible;

            HillclmLBL.Visibility = Visibility.Collapsed;
            EnakPomik.Visibility = Visibility.Collapsed;
            SimannealLBL1.Visibility = Visibility.Collapsed;
            ZacTmp.Visibility = Visibility.Collapsed;
            SimannealLBL2.Visibility = Visibility.Collapsed;
            SprTemp.Visibility = Visibility.Collapsed;
            GenalLBL1.Visibility = Visibility.Collapsed;
            VelPop.Visibility = Visibility.Collapsed;
            GenalLBL2.Visibility = Visibility.Collapsed;
            Elita.Visibility = Visibility.Collapsed;
            GenalLBL3.Visibility = Visibility.Collapsed;
            Krizanje.Visibility = Visibility.Collapsed;
            GenalLBL4.Visibility = Visibility.Collapsed;
            Mutacije.Visibility = Visibility.Collapsed;
            GenalLBL5.Visibility = Visibility.Collapsed;
            Generacije.Visibility = Visibility.Collapsed;
        }

        private void Genal_Checked(object sender, RoutedEventArgs e)
        {
            GenalLBL1.Visibility = Visibility.Visible;
            VelPop.Visibility = Visibility.Visible;
            GenalLBL2.Visibility = Visibility.Visible;
            Elita.Visibility = Visibility.Visible;
            GenalLBL3.Visibility = Visibility.Visible;
            Krizanje.Visibility = Visibility.Visible;
            GenalLBL4.Visibility = Visibility.Visible;
            Mutacije.Visibility = Visibility.Visible;
            GenalLBL5.Visibility = Visibility.Visible;
            Generacije.Visibility = Visibility.Visible;

            HillclmLBL.Visibility = Visibility.Collapsed;
            EnakPomik.Visibility = Visibility.Collapsed;
            SimannealLBL1.Visibility = Visibility.Collapsed;
            ZacTmp.Visibility = Visibility.Collapsed;
            SimannealLBL2.Visibility = Visibility.Collapsed;
            SprTemp.Visibility = Visibility.Collapsed;
            LBSLBL1.Visibility = Visibility.Collapsed;
            StStanj.Visibility = Visibility.Collapsed;
            LBSLBL2.Visibility = Visibility.Collapsed;
            StIterac.Visibility = Visibility.Collapsed;
        }

        int currentboardsize = 0;
        int[] startingposition;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StkorakovUI.Content = 0;
            ChessBoard.Children.RemoveRange(0, currentboardsize * currentboardsize);
            string Value = chesssize.SelectedValue.ToString();
            string sizestr = Value.Substring(0, Value.IndexOf('x'));
            int size = Convert.ToInt32(sizestr);    //Dobim željeno velikost sahovnice
            int[] Qpos = GenerateQueenPos(size);
            Checkerboard(Qpos);
            currentboardsize = Qpos.Length;
            startingposition = Qpos;
            int hh = IzrHevristike(Qpos);
            IzpHevr.Content = hh;
        }
    }
}
