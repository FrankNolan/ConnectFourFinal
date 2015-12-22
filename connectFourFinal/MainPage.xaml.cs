using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace connectFourFinal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //declare objects and variables
        int player1Score = 0;
        int player2Score = 0;
        Border square;
        int playerMove = 1;
      //  int gameState;
        int currentCol;
        int currentRow;
        //boolean array for coins
        int[,] coinArray = new int[6, 7];


        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            playerScores.Text = "Purple Score: 0 - Red Score: 0";
            initialseGame();
        }

        private void initialseGame()
        {
            stkpanel.Tapped -= StackPanel_Tapped;
            //create the board
            TextBlock text;

            //nested for loop to add squares to the board
            for (int rows = 0; rows < 6; rows++)
            {
                for (int cols = 0; cols < 7; cols++)
                {
                    //set the default value of coins to false
                    coinArray[rows, cols] = 0;

                    //add square to the board
                    square = new Border();
                    square.Name = "R" + rows.ToString() + "C" + cols.ToString();
                    //add event handler to top square 
                    if (rows == 0)
                    {
                        square.Tapped += Square_Tapped;
                    }
                       

                    text = new TextBlock();

                    square.SetValue(Grid.RowProperty, rows);
                    square.SetValue(Grid.ColumnProperty, cols);

                    board.Children.Add(square);
                    square.Height = 56.666;
                    square.Width = 104;
                    square.Background = new SolidColorBrush(Color.FromArgb(0xFF, 191, 185, 182)); // GREY

                    text.SetValue(Grid.RowProperty, square.GetValue(Grid.RowProperty));
                    text.SetValue(Grid.ColumnProperty, square.GetValue(Grid.ColumnProperty));
                    // set the alignment
                    text.HorizontalAlignment = HorizontalAlignment.Center;
                    text.VerticalAlignment = VerticalAlignment.Center;
                    square.Child = text;
                }//nested for
            }//outter for

        }

        private void Square_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //current object 
            Border curr = (Border)sender;
            currentCol = Convert.ToInt32(
                curr.Name.Substring(curr.Name.IndexOf("C") + 1));
            currentRow = Convert.ToInt32(
                curr.Name.Substring(1, curr.Name.IndexOf("C") - 1));

            //border to change color 
            Border findBorder;
            for (int i = 5; i >= 0; i--)
            {
                if (coinArray[i, currentCol] == 0)
                {

                    findBorder = (Border)board.FindName("R" + i.ToString() + "C" + currentCol.ToString());
                    if (playerMove == 1)
                    {
                        coinArray[i, currentCol] = 1;
                        findBorder.Background = new SolidColorBrush(Color.FromArgb(0xFF, 134, 53, 181)); // PURPLE

                        if (checkWinner() == 1) {
                            player1Score++;
                            playerScores.Text = "Purple Score: " + player1Score + " - Red Score: " + player2Score;
                            winner.Text = "Purple player won!";
                            clearBoard();
                            playerMove = 1;
                        }
                        
                    }
                    else
                    {
                        coinArray[i, currentCol] = 2;
                        findBorder.Background = new SolidColorBrush(Color.FromArgb(0xFF, 214, 15, 15)); // RED
                        if (checkWinner() == 2) {
                            player2Score++;
                            playerScores.Text = "Purple Score: " + player1Score + " - Red Score: " + player2Score;
                            winner.Text = "Purple player won!";
                            clearBoard();
                            playerMove = 1;
                        }
                    }

                    i = -1;
                }
            }


            if (playerMove == 1)
            {
                playerMove = 2;
            }
            else
            {
                playerMove = 1;
            }

        }
        /*
        private void checkGameState(int state)
        {
            if (state == 1 || state == 2)
            {
                Border findBorder;
                TextBlock findTextBlock;

                for (int i = 0; i < 6; i++)
                    for (int j = 0; j < 7; j++)
                    {
                        findBorder = (Border)board.FindName("R" + i.ToString() + "C" + j.ToString());
                        findTextBlock = findBorder.Child as TextBlock;
                        findTextBlock.Text = "You Win";
                    }
             }
        }

    */
        private void clearBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    coinArray[i, j] = 0; // reset all values to 0
                }
            }
            stkpanel.Tapped += StackPanel_Tapped;
        }

        private int checkWinner()
        {

            //check horizontal 
            for (int rows = 0; rows < 6; rows++)
            {
                
                for (int cols = 0; cols < 7; cols++)
                {
             
                    try {
                        // HORIZONTAL CHECKING
                        // check horizontal right
                        if (coinArray[rows, cols] == coinArray[rows, cols + 1]
                            && coinArray[rows, cols] == coinArray[rows, cols + 2]
                            && coinArray[rows, cols] == coinArray[rows, cols + 3]
                            && coinArray[rows,cols] != 0) {
                            return coinArray[rows, cols];
                        }
                        // check horizontal left
                        if (coinArray[rows, cols] == coinArray[rows, cols - 1]
                           && coinArray[rows, cols] == coinArray[rows, cols - 2]
                           && coinArray[rows, cols] == coinArray[rows, cols - 3]
                           && coinArray[rows, cols] != 0)
                        {
                            return coinArray[rows, cols];
                        }
                        // HORIZONTAL CHECKING


                    } // try
                    catch (IndexOutOfRangeException) {
                        continue;
                    } // catch


                    try {

                        // VERTICAL CHECKING

                        if (coinArray[rows, cols] == coinArray[rows - 1, cols]
                            && coinArray[rows, cols] == coinArray[rows - 2, cols]
                            && coinArray[rows, cols] == coinArray[rows - 3, cols]
                            && coinArray[rows, cols] != 0)
                        {
                            return coinArray[rows, cols];
                        }

                        if (coinArray[rows, cols] == coinArray[rows + 1, cols]
                           && coinArray[rows, cols] == coinArray[rows + 2, cols]
                           && coinArray[rows, cols] == coinArray[rows + 3, cols]
                           && coinArray[rows, cols] != 0)
                        {
                            return coinArray[rows, cols];
                        }
                        // VERTICAL CHECKING
                    }
                    catch (IndexOutOfRangeException) {
                        continue;
                    }

                    try {
                        // DIAGONAL CHECKING
                        if (coinArray[rows, cols] == coinArray[rows - 1, cols + 1]
                            && coinArray[rows, cols] == coinArray[rows - 2, cols + 2]
                            && coinArray[rows, cols] == coinArray[rows - 3, cols + 3]
                            && coinArray[rows, cols] != 0)
                        {
                            return coinArray[rows, cols];
                        }

                        if (coinArray[rows, cols] == coinArray[rows - 1, cols - 1]
                         && coinArray[rows, cols] == coinArray[rows - 2, cols - 2]
                         && coinArray[rows, cols] == coinArray[rows - 3, cols - 3]
                         && coinArray[rows, cols] != 0)
                        {
                            return coinArray[rows, cols];
                        }


                        if (coinArray[rows, cols] == coinArray[rows + 1, cols + 1]
                            && coinArray[rows, cols] == coinArray[rows + 2, cols + 2]
                            && coinArray[rows, cols] == coinArray[rows + 3, cols + 3]
                            && coinArray[rows, cols] != 0)
                        {
                            return coinArray[rows, cols];
                        }

                        if (coinArray[rows, cols] == coinArray[rows + 1, cols - 1]
                        && coinArray[rows, cols] == coinArray[rows + 2, cols - 2]
                        && coinArray[rows, cols] == coinArray[rows + 3, cols - 3]
                        && coinArray[rows, cols] != 0)
                        {
                            return coinArray[rows, cols];
                        }

                        // DIAGONAL CHECKING

                    }
                    catch (IndexOutOfRangeException) {
                        continue;
                    }

                } // inner for
            } // outer for


            return 0; // if there was no set of 4 found, there is no winner
        } // check winner

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            winner.Text = "";
            initialseGame();
        }
    }
    
}
