using Codurance.Domain;
using Codurance.Domain.Autofac;
using System.Windows;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var ticTacToe = TicTacToeFactory.Create(Team.Zero);
            var mainWindow = new MainWindow(ticTacToe);
            mainWindow.ShowDialog();
        }
    }
}
