using System;
using System.Collections.Generic;
using System.IO;
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

namespace Quiz
{
    public partial class MainWindow : Window
    {

        class Kerdesek
        {
            string kerdes;
            string[] valaszok;
            List<string> megoldasok = new List<string>();
            int sorszam;

            public Kerdesek(string kerdes, List<string> valaszok, int sorszam)
            {
                this.sorszam = sorszam;
                this.kerdes = kerdes.Substring(7, kerdes.Length - 7);
                if (this.kerdes[0] == ' ')
                {
                    this.kerdes = this.kerdes.Substring(1, this.kerdes.Length - 1);
                }
                this.valaszok = valaszok.ToArray();

                for (int i = 0; i < this.valaszok.Length; i++)
                {
                    if (this.valaszok[i][0] == '*')
                    {
                        this.valaszok[i] = this.valaszok[i].Substring(1, this.valaszok[i].Length - 1);
                        megoldasok.Add(this.valaszok[i]);
                    }
                }

            }

            public string[] Valaszok { get => valaszok; set => valaszok = value; }
            public string Kerdes { get => kerdes; set => kerdes = value; }
            public List<string> Megoldasok { get => megoldasok; set => megoldasok = value; }
            public int Sorszam { get => sorszam; set => sorszam = value; }


            public void Kever()
            {
                Random rand = new Random();
                int n = valaszok.Length;
                while (n > 1)
                {
                    n--;
                    int k = rand.Next(n + 1);
                    string value = valaszok[k];
                    valaszok[k] = valaszok[n];
                    valaszok[n] = value;
                }
            }
        }

        List<Kerdesek> kerdesek = new List<Kerdesek>();
        List<CheckBox> checkBoxes = new List<CheckBox>();
        List<int> voltMar = new List<int>();
        int kerdesSorszam = 0;
        Kerdesek elozoKerdes;
        List<Kerdesek> rosszak = new List<Kerdesek>();

        int index = 0;
        int minIndex = 0;
        int maxIndex = 10;

        public MainWindow()
        {
            InitializeComponent();

            StreamReader sr = new StreamReader("kerdesek.txt");
            string sor = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                string kerdes = "";
                List<string> valaszok = new List<string>();
                if (sor.Contains("Kérdés"))
                {
                    kerdes = sor;
                }
                try
                {
                    do
                    {
                        sor = sr.ReadLine();
                        if (!sor.Contains("Kérdés"))
                        {
                            valaszok.Add(sor);

                        }
                    }
                    while (!sor.Contains("Kérdés"));
                }
                catch (Exception)
                {
                }

                kerdesek.Add(new Kerdesek(kerdes, valaszok, kerdesSorszam));
                kerdesSorszam++;
            }
            sr.Close();

            maxIndex = kerdesek.Count;

            RandomKerdes();
        }

        void RandomKerdes()
        {
            if (kerdesek.Count > 0)
                elozoKerdes = kerdesek[index];

            sp.Children.Clear();
            checkBoxes.Clear();

            if (sorbanChk.IsChecked == true)
            {
                if (voltMar.Count > 0)
                {
                    if (maxIndex != -1)
                    {
                        do
                        {
                            IndexSzamol();
                        } while (voltMar.Contains(index));
                    }
                }
                else
                {
                    IndexSzamol();
                }
            }
            else
            {
                index++;
            }

            if (rosszakChk.IsChecked == true)
            {
                KerdesAd(rosszak);
            }
            else
            {
                KerdesAd(kerdesek);
            }

            if (maxIndex != -1)
            {
                int mennyi = maxIndex - minIndex;

                if (voltMar.Count == mennyi)
                {
                    for (int i = 0; i < (int)(mennyi * .5); i++)
                    {
                        voltMar.RemoveAt(i);
                    }
                }
            }
            else
            {
                voltMar.Clear();
            }

        }

        void IndexSzamol()
        {
            Random rand = new Random();
            if (rosszakChk.IsChecked == true)
            {
                if (rosszak.Count > 1)
                {
                    index = rand.Next(0, rosszak.Count);
                }
            }
            else
            {
                if (maxIndex == -1)
                {
                    index = minIndex;
                }
                else
                {
                    index = rand.Next(minIndex, maxIndex);
                }
            }

            rosszakLbl.Content = "";
            foreach (var item in rosszak)
            {
                rosszakLbl.Content += item.Sorszam + "\n";
            }
        }

        void KerdesAd(List<Kerdesek> lista)
        {
            lista[index].Kever();

            kerdesLbl.Text = lista[index].Kerdes;
            foreach (var item in lista[index].Valaszok)
            {
                CheckBox ck = new CheckBox() { Content = item };
                checkBoxes.Add(ck);
                ck.FontSize = 16;
                ck.VerticalContentAlignment = VerticalAlignment.Center;
                sp.Children.Add(ck);
            }

            voltMar.Add(lista[index].Sorszam);
            kerdesSorszamLbl.Content = lista[index].Sorszam;
        }

        void Ellenoriz()
        {
            int pontszamok = 0;
            int pipak = 0;
            foreach (var jelolt in checkBoxes)
            {
                if (jelolt.IsChecked == true)
                {
                    pipak++;
                }
                foreach (var megoldas in kerdesek[index].Megoldasok)
                {
                    if (megoldas == jelolt.Content)
                    {
                        jelolt.FontWeight = FontWeights.Bold;
                        if (jelolt.IsChecked == true)
                        {
                            pontszamok++;
                        }
                    }
                }
            }
            bool rossz = false;
            pontszamLbl.Content = string.Format("{0}/{1}/{2}", pipak, pontszamok, kerdesek[index].Megoldasok.Count);
            if (pipak > kerdesek[index].Megoldasok.Count)
            {
                rossz = true;
            }
            else
            {
                if (pontszamok < kerdesek[index].Megoldasok.Count)
                {
                    rossz = true;
                }
                else
                {
                    rossz = false;
                }
            }
            if (rossz)
            {
                pontszamLbl.Foreground = Brushes.Red;
                rosszak.Add(kerdesek[index]);
                StreamWriter sw = new StreamWriter("rosszak.txt",true);
                sw.WriteLine(kerdesek[index].Sorszam);
                sw.Close();
            }
            else
            {
                pontszamLbl.Foreground = Brushes.Green;
            }


        }

        private void MehetBtn_Click(object sender, RoutedEventArgs e)
        {
            Ellenoriz();
        }

        private void KovetkezoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string range = rangeTxtB.Text;
                if (!range.Contains('-'))
                {
                    minIndex = Convert.ToInt32(range);
                    if (minIndex > kerdesek.Count)
                    {
                        minIndex = kerdesek.Count - 1;
                        MessageBox.Show("Maximális kérdések száma: " + kerdesek.Count);
                        rangeTxtB.Text = string.Format("{0}", minIndex);
                    }
                    maxIndex = -1;
                }
                else
                {
                    string[] indexes = range.Split('-');
                    minIndex = Convert.ToInt32(indexes[0]);
                    maxIndex = Convert.ToInt32(indexes[1]);
                    if (maxIndex > kerdesek.Count)
                    {
                        maxIndex = kerdesek.Count - 1;
                        MessageBox.Show("Maximális kérdések száma: " + kerdesek.Count);
                        rangeTxtB.Text = string.Format("{0}-{1}", minIndex, maxIndex);
                    }
                }

            }
            catch (Exception)
            {
            }

            pontszamLbl.Content = "";

            RandomKerdes();
        }

        private void ElozoKerdesBtn_Click(object sender, RoutedEventArgs e)
        {
            sp.Children.Clear();
            checkBoxes.Clear();

            elozoKerdes.Kever();

            kerdesLbl.Text = elozoKerdes.Kerdes;

            foreach (var item in elozoKerdes.Valaszok)
            {
                CheckBox ck = new CheckBox() { Content = item };
                checkBoxes.Add(ck);
                ck.FontSize = 16;
                ck.VerticalContentAlignment = VerticalAlignment.Center;
                sp.Children.Add(ck);
            }

            index = elozoKerdes.Sorszam;
        }

        private void SorbanChk_Checked(object sender, RoutedEventArgs e)
        {
            if (sorbanChk.IsChecked == true)
            {
                sorbanChk.Content = "Random";
            }
            else
            {
                sorbanChk.Content = "Sorban";
            }
        }

        private void RosszakChk_Checked(object sender, RoutedEventArgs e)
        {
            if (rosszakChk.IsChecked == true)
            {
                rosszakChk.Content = "Rosszak";
            }
            else
            {
                rosszakChk.Content = "Jók";
            }
        }

        private void RosszakTorolBtn_Click(object sender, RoutedEventArgs e)
        {
            rosszak.Clear();
            StreamWriter sw = new StreamWriter("rosszak.txt");
            sw.WriteLine("");
            sw.Close();
        }
    }
}
