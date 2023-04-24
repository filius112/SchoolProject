using System;
using System.Collections.Generic;
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

namespace KeyDown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
         DispatcherTimer hod = new DispatcherTimer();
            hod.Interval = new TimeSpan(0,0, 6/3);


            InitializeComponent();
        }

        int score = 0;

        private void main_Click(object sender, RoutedEventArgs e)
        {
            score += 1;

            Count.Text = score.ToString();
        }

        private async void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int i = 0;
            await Task.Delay(1000);

            Count.Text = score.ToString();
        }
    }
}
