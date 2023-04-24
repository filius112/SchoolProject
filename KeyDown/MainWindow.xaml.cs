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



            this.KeyDown += (s, e) =>
            {
                var klavesa = e.Key;
                switch (klavesa)
                {
                    case Key.W:
                        Canvas.SetTop(Elipsa, Canvas.GetTop(Elipsa) - 10);
                        
                        break;
                    case Key.A:
                      Canvas.SetLeft(Elipsa, Canvas.GetLeft(Elipsa) - 10);
                        break;
                    case Key.S:
                        Canvas.SetTop(Elipsa, Canvas.GetTop(Elipsa) + 10);
                        break;
                        case Key.D:
                        
                        Canvas.SetLeft(Elipsa, Canvas.GetLeft(Elipsa) + 10);
                        break;
                }

            };
        }
        
        private void Collision()
        {
            int i = 0;
            if (i == 0)
            {
            }
        }

        private void Change()
        {
            Canvas.SetTop(Elipsa, Left);
        }
    }
}
