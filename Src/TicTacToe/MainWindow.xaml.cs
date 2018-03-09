using Codurance.Domain;
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

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ITicTacToeGame ticTacToeGame;

        public MainWindow()
        {
            InitializeComponent();
            // TODO: Uset Autofac and inject game
            this.ticTacToeGame = new TicTacToeGame(new Board());            
            // var bitmapImage = new BitmapImage(new Uri($"{Environment.CurrentDirectory}\\Images\\Frame.png"));
            // imgFrame.Source = bitmapImage;
        }
    }
}
