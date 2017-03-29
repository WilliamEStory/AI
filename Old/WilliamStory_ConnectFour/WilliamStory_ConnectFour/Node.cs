using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilliamStory_ConnectFour
{
    class Node
    {
        public List<Node> childList { get; set; }
        public double heuristic { get; set; }
        public int[,] currentBoard;
        public int childDepth;
        public int columnPlayed;
        public bool player;
        public int countNeeded;

        public Node(int rows, int columns, int childDepth, int columnPlayed, bool player, int countNeeded)
        {
            childList = new List<Node>();
            currentBoard = new int[rows, columns];
            heuristic = -100;
            this.childDepth = childDepth;
            this.columnPlayed = columnPlayed;
            this.player = player;
            this.countNeeded = countNeeded;
        }

        public void generateHeuristic()
        {
            heuristic = 0;
            int playerAsInt = player ? 1 : 0;
            bool otherPlayer = !player;
            int otherPlayerAsInt = otherPlayer ? 1 : 0;

            int connectR = 0;
            double newHeuristic = 0;
            int amtRows = currentBoard.GetLength(0) - 1;

            #region CHECK_MY_WIN
            newHeuristic = 0;
            for (int row = amtRows; row > 0; row--)
            {
                for (int column = 0; column < currentBoard.GetLength(0); column++)
                {
                    if (currentBoard[row, column] == -1)
                    {
                        break;
                    }
                    //Check the column
                    #region COLUMN_CHECK
                    for (int rows = 0; rows < currentBoard.GetLength(0); rows++)
                    {
                        if (currentBoard[rows, column] == playerAsInt)
                        {
                            connectR++;
                        }
                        else
                        {
                            newHeuristic = connectR / countNeeded;
                            int rowAbove = rows + 1;
                            if (rowAbove == currentBoard.GetLength(0))
                            {
                                rowAbove = currentBoard.GetLength(0) - 1;
                            }
                            if (currentBoard[rowAbove, column] == -1)
                            {
                                newHeuristic += .05;
                            }
                            else
                            {
                                newHeuristic -= .05;
                            }
                            int colLeft = column - 1;
                            //Reset column if out of bounds
                            if (colLeft < 0)
                            {
                                colLeft = 0;
                            }
                            int colRight = column + 1;
                            //reset column if out of bounds
                            if (colRight > currentBoard.GetLength(1))
                            {
                                colRight = currentBoard.GetLength(1) - 1;
                            }
                            if (currentBoard[rows, colLeft] == -1)
                            {
                                newHeuristic += .05;
                            }
                            else
                            {
                                newHeuristic -= .05;
                            }
                            if (currentBoard[rows, colRight] == -1)
                            {
                                newHeuristic += .05;
                            }
                            else
                            {
                                newHeuristic -= .05;
                            }
                            if (newHeuristic > heuristic)
                            {
                                heuristic = newHeuristic;
                            }
                            connectR = 0;
                        }
                        if (connectR == countNeeded)
                        {
                            heuristic = 1;
                        }
                    }
                    #endregion
                    connectR = 0;
                    //Check the row
                    #region ROWCHECK
                    for (int columns = 0; columns < currentBoard.GetLength(1); columns++)
                    {
                        if (currentBoard[row, columns] == playerAsInt)
                        {
                            connectR++;
                        }
                        else
                        {
                            newHeuristic = connectR / countNeeded;
                            int rowAbove = row + 1;
                            if (rowAbove == currentBoard.GetLength(0))
                            {
                                rowAbove = currentBoard.GetLength(0) - 1;
                            }
                            if (currentBoard[rowAbove, column] == -1)
                            {
                                newHeuristic += .05;
                            }
                            else
                            {
                                newHeuristic -= .05;
                            }
                            int colLeft = column - 1;
                            //Reset column if out of bounds
                            if (colLeft < 0)
                            {
                                colLeft = 0;
                            }
                            int colRight = column + 1;
                            //reset column if out of bounds
                            if (colRight > currentBoard.GetLength(1))
                            {
                                colRight = currentBoard.GetLength(1) - 1;
                            }
                            if (currentBoard[row, colLeft] == -1)
                            {
                                newHeuristic += .05;
                            }
                            else
                            {
                                newHeuristic -= .05;
                            }
                            if (currentBoard[row, colRight] == -1)
                            {
                                newHeuristic += .05;
                            }
                            else
                            {
                                newHeuristic -= .05;
                            }
                            if (newHeuristic > heuristic)
                            {
                                heuristic = newHeuristic;
                            }
                            if (countNeeded == connectR)
                            {
                                this.heuristic = 1;
                            }
                            connectR = 0;
                        }
                    }
                    #endregion
                }
            }
            #endregion
            #region CHECK_OTHER_PLAYER_WIN
            //If other player has won this board, don't play it.
            //Give either a really low heuristic or -1 to try and avoid play.
            for (int row = 0; row < amtRows; row++ )
            {
                for (int col = 0; col < currentBoard.GetLength(1); col++)
                {
                    if (currentBoard[row, col] == otherPlayerAsInt)
                    {
                        bool win = checkWin(row, col, otherPlayer);
                        if (win == false)
                        {
                            this.heuristic = -1.0;
                            return;
                        }
                    }
                }
            }
            #endregion
        }

        private bool checkWin(int startRow, int startColumn, bool player)
        {
            bool winFound = false;
            int playerAsInt = player ? 1 : 0;
            int countNeeded = 0;
            int columns = currentBoard.GetLength(1);
            int connectR = countNeeded;
            int rows = currentBoard.GetLength(0);

            bool win = true;

            #region CHECK_ROW_COLUMN
            //Check row
            for (int col = 0; col < columns; col++)
            {
                if (currentBoard[startRow, col] == playerAsInt)
                {
                    countNeeded++;
                }
                else
                {
                    countNeeded = 0;
                }
                if (countNeeded == connectR)
                {
                    return win;
                }
            }

            //Check column
            countNeeded = 0; //Reset needed count.
            //Only need to check columns going downwards
            for (int row = startRow; row < rows; row++)
            {
                if (currentBoard[row, startColumn] == playerAsInt)
                {
                    countNeeded++;
                }
                else
                {
                    countNeeded = 0;
                }
                if (countNeeded == connectR)
                {
                    return win;
                }
            }

            #endregion

            //Check Diagonal
            #region CHECK_DIAGONAL_UPPERLEFT_LOWERRIGHT
            //This is the upper left to lower right
            countNeeded = 0;
            int rowHolder = startRow;
            int colHolder = startColumn;
            //First get to as far as upper left as 
            //player chain goes.
            while (colHolder > 0)
            {
                colHolder--;
                rowHolder--;

                if (rowHolder < 0)
                {
                    rowHolder++;
                    colHolder++;
                    break;
                }
                else
                {

                    if (currentBoard[rowHolder, colHolder] == playerAsInt)
                    {
                        countNeeded++;
                        if (countNeeded == connectR)
                        {
                            return win;
                        }
                    }
                    else
                    {
                        //Didn't find a connectR.
                        countNeeded = 0;
                        //Increment row/column so we start our check on the right
                        //cell.
                        rowHolder++;
                        colHolder++;
                        break;
                    }
                }
            }

            //Now check going from upper left to lower right.
            countNeeded = 0;
            while (colHolder < columns)
            {
                if (rowHolder >= rows)
                {
                    break;
                }

                if (currentBoard[rowHolder, colHolder] == playerAsInt)
                {
                    countNeeded++;
                }
                else
                {
                    break;
                }
                if (countNeeded == connectR)
                {
                    return win;
                }



                rowHolder++;

                colHolder++;
            }

            #endregion
            #region CHECK_DIAGONAL_LOWERLEFT_UPPERRIGHT
            countNeeded = 0;
            rowHolder = startRow;
            colHolder = startColumn;
            while (colHolder > 0)
            {
                colHolder--;
                rowHolder++;

                if (rowHolder >= rows)
                {
                    rowHolder--;
                    colHolder++;
                    break;
                }
                else
                {

                    if (currentBoard[rowHolder, colHolder] == playerAsInt)
                    {
                        countNeeded++;
                        //If we just so happen to find a connectR chain
                        //on the way to actually setting up to check the diagonal.
                        //Why check the chain twice?
                        if (countNeeded == connectR)
                        {
                            return win;
                        }
                    }
                    else
                    {
                        //Didn't find a connectR.
                        countNeeded = 0;
                        //Increment row/column so we start our check on the right
                        //cell.
                        rowHolder--;
                        colHolder++;
                        break;
                    }
                }
            }
            countNeeded = 0;
            //Now check going from upper left to lower right.
            while (colHolder < columns)
            {
                if (rowHolder <= 0)
                {
                    break;
                }

                if (currentBoard[rowHolder, colHolder] == playerAsInt)
                {
                    countNeeded++;
                }
                else
                {
                    break;
                }
                if (countNeeded == connectR)
                {
                    return win;
                }



                rowHolder--;

                colHolder++;
            }
            #endregion
            if (countNeeded > 0)
            {
                winFound = false;
            }
            return winFound;
        }
    }
}
