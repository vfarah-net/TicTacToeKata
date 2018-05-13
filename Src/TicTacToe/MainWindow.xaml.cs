using Codurance.Domain;
using Codurance.Domain.Autofac;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TicTacToe
{
    /// <summary>
    /// Very simple UI to demonstrate the tic-tac-toe game logic
    /// </summary>
    /// <remarks>
    /// This does not respect proper WPF patterns and is purely a simple 
    /// test harness for the tic tac toe game
    /// </remarks>
    public partial class MainWindow : Window
    {
        private readonly ITicTacToeGame ticTacToeGame;
        private Team currentTeam = Team.Zero;
        private readonly RotateTransform rotateVertically = new RotateTransform(90.00);
        private readonly RotateTransform rotateLeftDiagnol = new RotateTransform(-135.00);
        private readonly RotateTransform rotateRightDiagnol = new RotateTransform(-45.00);        
        private readonly Point rotateFromMiddle = new Point(0.5, 0.5);
        private readonly BitmapImage bitmapImageTeamZero = new BitmapImage();
        private readonly BitmapImage bitmapImageTeamCross = new BitmapImage();
        private readonly BitmapImage bitmapImageEmpty = new BitmapImage();
        private readonly BitmapImage bitmapZeroLine = new BitmapImage();
        private readonly BitmapImage bitmapCrossLine = new BitmapImage();

        public MainWindow():this(TicTacToeFactory.Create(Team.Zero))
        { }

        public MainWindow(ITicTacToeGame ticTacToeGame)
        {
            this.ticTacToeGame = ticTacToeGame;            
            ticTacToeGame.ScoreChanged += OnScoreChanged;
            ticTacToeGame.TeamAndLineWon += OnTeamAndLineWon;
            ticTacToeGame.TeamChanged += OnTeamChanged;
            InitializeComponent();
            InitializeBitmapImages();
            OnTeamChanged(currentTeam);
            ClearImages();
        }

        private void ClearImages()
        {
            imgBottomLeft.Source = bitmapImageEmpty;
            imgBottomMiddle.Source = bitmapImageEmpty;
            imgBottomRight.Source = bitmapImageEmpty;
            imgMiddle.Source = bitmapImageEmpty;
            imgMiddleLeft.Source = bitmapImageEmpty;
            imgMiddleRight.Source = bitmapImageEmpty;
            imgTopLeft.Source = bitmapImageEmpty;
            imgTopMiddle.Source = bitmapImageEmpty;
            imgTopRight.Source = bitmapImageEmpty;
        }

        private void InitializeBitmapImages()
        {
            bitmapImageTeamCross.BeginInit();
            bitmapImageTeamCross.UriSource = new Uri("pack://siteoforigin:,,,/Images/Cross.png");
            bitmapImageTeamCross.EndInit();

            bitmapImageTeamZero.BeginInit();
            bitmapImageTeamZero.UriSource = new Uri("pack://siteoforigin:,,,/Images/Naught.png");
            bitmapImageTeamZero.EndInit();

            bitmapImageEmpty.BeginInit();
            bitmapImageEmpty.UriSource = new Uri("pack://siteoforigin:,,,/Images/Empty.png");
            bitmapImageEmpty.EndInit();

            bitmapZeroLine.BeginInit();
            bitmapZeroLine.UriSource = new Uri("pack://siteoforigin:,,,/Images/ZeroLine.png");
            bitmapZeroLine.EndInit();

            bitmapCrossLine.BeginInit();
            bitmapCrossLine.UriSource = new Uri("pack://siteoforigin:,,,/Images/CrossLine.png");
            bitmapCrossLine.EndInit();
        }

        private void OnTeamChanged(Team team)
        {
            this.currentTeam = team;
            imgCurrentTeam.Source =
                team == Team.Cross ? bitmapImageTeamCross : bitmapImageTeamZero;
        }

        private void OnTeamAndLineWon(ValueTuple<Team, LineWin> teamAndLineWin)
        {
            btnReset.Visibility = Visibility.Visible;
            ShowLine(teamAndLineWin);
        }

        private void ResetGame()
        {

            ClearImages();
            ticTacToeGame.Reset();
            btnReset.Visibility = Visibility.Collapsed;
            HideLine();
        }

        private void OnScoreChanged(IReadOnlyDictionary<Team, int> scores)
        {
            lblTeamCrossScore.Content = scores[Team.Cross].ToString();
            lblTeamZeroScore.Content = scores[Team.Zero].ToString();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ticTacToeGame.IsGameOver) return;
            if (sender is Image image)
            {                
                var currentTeamBitmap = currentTeam == Team.Zero ? bitmapImageTeamZero : bitmapImageTeamCross;
                if(Enum.TryParse(image.Uid, out BoardPosition position))
                {
                    if (ticTacToeGame.Move(position))
                    {
                        image.Source = currentTeamBitmap;
                    }
                } else
                {
                    throw new NotSupportedException(image.Uid);
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        private void HideLine()
        {
            imgLine.Visibility = Visibility.Collapsed;
        }

        private void ShowLine(ValueTuple<Team, LineWin> teamAndLineWinner)
        {
            if (teamAndLineWinner.Item1 != Team.None &&
                teamAndLineWinner.Item2 != LineWin.None)
            {
                imgLine.Source = teamAndLineWinner.Item1 == Team.Cross ? bitmapCrossLine : bitmapZeroLine;
                imgLine.Height = 13;
                imgLine.Width = 291;
                imgLine.Stretch = Stretch.Uniform;
                imgLine.StretchDirection = StretchDirection.Both;
                imgLine.RenderTransform = null;
                switch (teamAndLineWinner.Item2)
                {
                    case LineWin.TopHorizontal:
                        {
                            SetupTopHorizontal();
                            break;
                        }
                    case LineWin.MiddleHorizontal:
                        {
                            SetupMiddleHorizontal();
                            break;
                        }
                    case LineWin.BottomHorizontal:
                        {
                            SetupBottomHorizontal();
                            break;
                        }
                    case LineWin.LeftVertical:
                        {
                            SetupLeftVertical();
                            break;
                        }
                    case LineWin.MiddleVertical:
                        {
                            SetupMiddleVertical();
                            break;
                        }
                    case LineWin.RightVertical:
                        {
                            SetupRightVertical();
                            break;
                        }
                    case LineWin.LeftDiagonal:
                        {
                            SetupLeftDiagonol();
                            break;
                        }
                    case LineWin.RightDiagonal:
                        {
                            SetupRightDiagonol();
                            break;
                        }
                    case LineWin.None:
                        break;
                }
                imgLine.Visibility = Visibility.Visible;
            }                        
        }

        private void SetupRightDiagonol()
        {            
            imgLine.Margin = new Thickness(50, 290, 0, 0);
            imgLine.RenderTransformOrigin = rotateFromMiddle;
            imgLine.RenderTransform = rotateRightDiagnol;
            Grid.SetColumnSpan(imgLine, 2);
        }

        private void SetupLeftDiagonol()
        {            
            imgLine.Margin = new Thickness(50, 284, 0, 0);
            imgLine.RenderTransformOrigin = rotateFromMiddle;
            imgLine.RenderTransform = rotateLeftDiagnol;
            Grid.SetColumnSpan(imgLine, 2);
        }

        private void SetupRightVertical()
        {
            // ISSUE: with the actual image not filling the entire space
            imgLine.Margin = new Thickness(155, 284, -51.132, 0);            
            imgLine.RenderTransformOrigin = rotateFromMiddle;
            imgLine.RenderTransform = rotateVertically;
            Grid.SetColumnSpan(imgLine, 2);
        }

        private void SetupMiddleVertical()
        {
            imgLine.Margin = new Thickness(53, 284, 0, 0);
            imgLine.RenderTransformOrigin = rotateFromMiddle;
            imgLine.RenderTransform = rotateVertically;
            Grid.SetColumnSpan(imgLine, 2);
        }

        private void SetupLeftVertical()
        {
            imgLine.Margin = new Thickness(-47, 284, 0, 0);
            imgLine.RenderTransformOrigin = rotateFromMiddle;
            imgLine.RenderTransform = rotateVertically;
        }

        private void SetupBottomHorizontal()
        {
            imgLine.Margin = new Thickness(50, 384, 0, 0);
        }

        private void SetupMiddleHorizontal()
        {
            imgLine.Margin = new Thickness(50, 288, 0, 0);
        }

        private void SetupTopHorizontal()
        {
            imgLine.Margin = new Thickness(50, 183, 0, 0);
        }
    }
}
