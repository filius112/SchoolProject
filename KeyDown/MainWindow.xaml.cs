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
                        ContentPosition.
                        break ;
                    case Key.A:
                        MessageBox.Show("Stiskl jsi A");
                        break;
                    case Key.S:
                        MessageBox.Show("Stiskl jsi S");
                        break;
                        case Key.D:
                        MessageBox.Show("Stiskl jsi D");
                        break;
                        default:
                        MessageBox.Show(e.Key.ToString());
                        break;
                }
               

            };
        }
    }
}
