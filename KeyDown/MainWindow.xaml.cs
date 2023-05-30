using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Data.SQLite;
using System.IO;
using System.Configuration;
using System.Xml.Linq;
using KeyDown.Models;
using System.Windows.Media.Animation;

namespace KeyDown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //DATABÁZE        

        //Název databáze
         const string saveScore = "MyDatabase.db";


        const string ConnectStr = $"Data Source={saveScore}; Version=3;";

        private void CreateDB()
        {
            if (!File.Exists(saveScore))
            {
                SQLiteConnection.CreateFile(saveScore);
                using (SQLiteConnection conn = new SQLiteConnection(ConnectStr))
                {
                    string savedScore = "CREATE TABLE IF NOT EXISTS Saves(" + "ID INTEGER PRIMARY KEY AUTOINCREMENT," + "score INTEGER NOT NULL);";
                    conn.Open();
                    SQLiteCommand cmd2 = new SQLiteCommand(savedScore, conn);
                    cmd2.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        void SeedData()
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectStr))
            {
                conn.Open();
                string cmd = "INSERT INTO Saves(score)VALUES" +
                    "(5)," +
                    "(103);";
                SQLiteCommand CMD = new SQLiteCommand(cmd, conn);
                CMD.ExecuteNonQuery();
                conn.Close();
                UpdateHiscore();
            }
        }

        private List<saves> ObtainData()
        {
            List<saves> data = new List<saves>();
            using (SQLiteConnection conn = new SQLiteConnection(ConnectStr))
            {
                string cmd = "SELECT * FROM Saves";
                conn.Open();
                using (SQLiteCommand sqlcmd = new SQLiteCommand(cmd, conn))
                {
                    SQLiteDataReader reader = sqlcmd.ExecuteReader();
                    while (reader.Read())
                    {
                        data.Add(new saves
                        {
                            score = int.Parse(reader["score"].ToString())
                        });
                    }
                }
                conn.Close();
            }
            return data;
        }
        void DeleteDB()
        {
            if (File.Exists(saveScore))
            {
                File.Delete(saveScore);
            }
        }

        List<saves> Saves { get; set; }



        DispatcherTimer dispatcherTimer = new DispatcherTimer();

       
        int count = 0;

        double score = 0;



        public MainWindow()
        {
            CreateDB();
            //DeleteDB();
            SeedData();
            Saves = ObtainData();
            InitializeComponent();
            UpdateHiscore();

            // Nastaví interval mezi jedním taktem v časovači
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);

            
            dispatcherTimer.Tick += DispatcherTimer_Tick;

            //Jitter button je deaktivovaný
            Jitter.IsEnabled = false;

        }


        void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            
            count++;

            //Podmínka, že pokud je count 10 nebo větší, tak se zastaví DispTimer
            if (count >= 10)
            {
                dispatcherTimer.Stop();
                startButton.IsEnabled = true; // startButton spouští čas. Tímto se aktivuje po tom, co byl během počítání deaktivovaný
                count = 0;
                Jitter.IsEnabled = false; //Jitter je opět deaktivovaný, aby se nemohlo přičítat více skóre bez času a omezení
                cps.Text = (score / 10 + " CPS").ToString();
                UpdateHiscore();
           
                
            }

            using (SQLiteConnection conn = new SQLiteConnection(ConnectStr))
            {
                conn.Open();

                string cmd = "INSERT INTO Saves(score) VALUES (@score);";

                using (SQLiteCommand sqlcmd = new SQLiteCommand(cmd, conn))
                {
                    sqlcmd.Parameters.AddWithValue("@score", score);
                    sqlcmd.ExecuteNonQuery();
                }

                conn.Close();
            }


            //Aktualizuje laabel.Content s int count převedeným na string
            laabel.Content = count.ToString();

            double currentCPS = double.Parse(cps.Text.Split(' ')[0]);
            double currentHiScore = double.Parse(hiscore.Text.Split(' ')[0]);

            if (currentCPS > currentHiScore)
            {
                hiscore.Text = cps.Text;
                MessageBox.Show("New High Score! Congrats!", "NEW HIGH SCORE", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            
            //Deaktivace startButton buttonu a aktivace Jitter Buttonu. Je to z důvodu, aby se nemohlo klikat na startButton vícekrát, během toho co běží čas
            startButton.IsEnabled = false;
            Jitter.IsEnabled = true;

            // Čas má interval 1 sekundu, což znamená, že než se spustí první sekunda, tak musí uběhnout 1 sekunda
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
            score = 0;
            Counter.Text = score.ToString();
        }

        private void Jitter_Click(object sender, RoutedEventArgs e)
        {
            score ++;

            Counter.Text = score.ToString();
        }
        private void UpdateHiscore()
        {
            if (Saves != null && Saves.Count > 0)
            {
                //Vytáhne data, které jsou uložené v "MyDatabase.db"
                Saves = ObtainData();

                // Vezme nejvyšší skóre
                double highestScore = Saves.Max(s => s.score);

                //highestScore z "MyDatabase.db" bude vloženo do "hiscore.Text". Vždy to načte nejvyšší skóre, takže nikdy neztratíte pojem o tom, co je Vaše nejvyšší skóre
                hiscore.Text = $"{highestScore/10} CPS";
            }
        }


    }
}
