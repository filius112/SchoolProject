using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace KeyDown
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
         

            InitializeComponent();

            this.KeyDown += (s, e) =>
            {
                switch (e.Key)
                {
                    case Key.Space:
                        MessageBox.Show("Stiskl jsi mezerník");
                        break;
                    case Key.W:
                        Canvas.SetTop(Elipsa, Canvas.GetTop(Elipsa) - 20);
                        break;
                    case Key.A:
                      Canvas.SetLeft(Elipsa, Canvas.GetLeft(Elipsa) - 20);
                        break;
                    case Key.S:
                        Canvas.SetTop(Elipsa, Canvas.GetTop(Elipsa) + 20);
                        break;
                    case Key.D:
                        Canvas.SetLeft(Elipsa, Canvas.GetLeft(Elipsa) + 20);
                        break;
                    default:
                        MessageBox.Show(e.Key.ToString());
                        break;
                }

            };
        }

        private void Elipsa_KeyDown(object sender, KeyEventArgs e)
        {
            

        }
        private void Change()
        {
            Canvas.SetTop(Elipsa, Left);
        }
    }
}
