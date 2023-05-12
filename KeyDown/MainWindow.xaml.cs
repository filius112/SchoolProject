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

namespace KeyDown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        // Specify the name of the new database file
        const string saveScore = "MyDatabase.db";

        // Create a new SQLite database file

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
                    "(10);";
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

        // Set the initial count to 0
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

            // Set the interval between timer ticks (in milliseconds)
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);

            // Set the event handler for the Tick event
            dispatcherTimer.Tick += DispatcherTimer_Tick;

            Jitter.IsEnabled = false;

        }

         void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Increment the count
            count++;

            // If the count is greater than or equal to 10, stop the timer
            if (count >= 10)
            {
                dispatcherTimer.Stop();
                startButton.IsEnabled = true; // Re-enable the button
                count = 0;
                Jitter.IsEnabled = false;
                cps.Text = (score / 10 + " clicks/s").ToString();
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


            // Update the TextBlock with the current count
            laabel.Content = count.ToString();

            double currentCPS = double.Parse(cps.Text.Split(' ')[0]);
            double currentHiScore = double.Parse(hiscore.Text.Split(' ')[0]);

            if (currentCPS > currentHiScore)
            {
                hiscore.Text = cps.Text;
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            
            // Disable the button while the timer is running
            startButton.IsEnabled = false;
            Jitter.IsEnabled = true;

            // Start the timer after 1 seconds
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
                // Obtain the saved data from the database
                Saves = ObtainData();

                // Calculate the highest score
                double highestScore = Saves.Max(s => s.score);

                // Set the Text property of the hiscore label
                hiscore.Text = $"{highestScore/10} clicks/s";
            }
        }


    }
}
