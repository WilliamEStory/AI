using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WilliamStory_ConnectFour
{
    public partial class Form1 : Form
    {
        int[,] boardState;
        int rows, columns, connectR;
        bool playerTurn;
        bool AI_VS_HUMAN;
        int depth;

        Node rootStart;

        public Form1()
        {
            InitializeComponent();
        }

        private void b_Generate_Click(object sender, EventArgs e)
        {
            rows = int.Parse(tb_Rows.Text);
            columns = int.Parse(tb_Columns.Text);
            connectR = int.Parse(tb_ConnectR.Text);
            depth = int.Parse(tb_Depth.Text);
            playerTurn = false; //Red goes first
            AI_VS_HUMAN = true;

            boardState = new int[rows, columns];
            rootStart = new Node(rows, columns, 0, -1, false, connectR);
            for (int rowCount = 0; rowCount < rows; rowCount++)
            {
                for (int columnCount = 0; columnCount < columns; columnCount++)
                {
                    boardState[rowCount, columnCount] = -1;
                    rootStart.currentBoard[rowCount, columnCount] = -1;
                }
            }

            DataTable board = new DataTable();
            for (int i = 0; i < columns; i++)
            {
                board.Columns.Add(new DataColumn(i + "", typeof(string)));
            }
            for (int i = 0; i < rows; i++)
            {
                board.Rows.Add();
            }
            dgv_Board.DataSource = board;
        }

        #region CHECK_WIN
        private bool checkWin(int startRow, int startColumn, bool player)
        {
            bool winFound = false;
            int playerAsInt = player ? 1 : 0;
            int countNeeded = 0;

            bool win = true;

            #region CHECK_ROW_COLUMN
            //Check row
            for (int col = 0; col < columns; col++)
            {
                if (boardState[startRow, col] == playerAsInt)
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
                if (boardState[row, startColumn] == playerAsInt)
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

                    if (boardState[rowHolder, colHolder] == playerAsInt)
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

                if (boardState[rowHolder, colHolder] == playerAsInt)
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

                    if (boardState[rowHolder, colHolder] == playerAsInt)
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

                if (boardState[rowHolder, colHolder] == playerAsInt)
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
        #endregion

        #region ADD_PLAY_CLICK_AND_ADD_PLAYER_MOVE
        private void b_AddColumn_Click(object sender, EventArgs e)
        {
            #region PLAYERVSAI
            if (cb_PlayerVsAI.Checked)
            {
                //AI Has already moved, so 
                //human will be black.
                if (cb_AIFirst.Checked)
                {
                    playerTurn = true;
                }
                //AI Is not going first, so 
                //human will be red and move first.
                else
                {
                    playerTurn = false;
                }
                int column = int.Parse(tb_Column.Text);
                if (column < 0 || column > columns)
                {
                    MessageBox.Show("Invalid placement choice");
                    return;
                }

                //Play the player's move
                //Check to see if it was a winning move.
                int rowPlaced = addPlayerMove(column, this.boardState, playerTurn, false);
                bool won = checkWin(rowPlaced, column, playerTurn);

                if (won)
                {
                    if (playerTurn)
                    {
                        MessageBox.Show("Black won!");
                    }
                    else
                    {
                        MessageBox.Show("Red won!");
                    }
                    b_Play.Enabled = false;
                    return;
                }

                //If we are playing an AI Vs. Human game.
                if (AI_VS_HUMAN == true)
                {
                    int AIPlacement = 0;
                    playerTurn = !playerTurn;
                    if (rootStart.childList.Count != 0)
                    {
                        rootStart.childList.Clear();
                    }
                    generateFutureBoards(rootStart, 0, depth, playerTurn);
                    makeHeuristic(rootStart, depth);
                    AIPlacement = miniMax(rootStart, depth, true).columnPlayed;
                    int AIRowPlayed = addPlayerMove(AIPlacement, this.boardState, playerTurn, false);
                    won = checkWin(AIRowPlayed, AIPlacement, playerTurn);




                    if (won)
                    {
                        if (playerTurn)
                        {
                            MessageBox.Show("Black won!");
                        }
                        else
                        {
                            MessageBox.Show("Red won!");
                        }
                        b_Play.Enabled = false;
                    }

                    playerTurn = !playerTurn;
                }
            }
            #endregion
            #region PLAYERVSPLAYER
            else
            {
                int column = int.Parse(tb_Column.Text);
                if (column < 0 || column > columns)
                {
                    MessageBox.Show("Invalid placement choice");
                    return;
                }

                int rowPlaced = addPlayerMove(column, this.boardState, playerTurn, false);
                bool won = checkWin(rowPlaced, column, playerTurn);

                if (won)
                {
                    if (playerTurn)
                    {
                        MessageBox.Show("Black won!");
                    }
                    else
                    {
                        MessageBox.Show("Red won!");
                    }
                    b_Play.Enabled = false;
                    return;
                }
                playerTurn = !playerTurn;

            }
            #endregion
        }

        private int addPlayerMove(int column, int[,] board, bool player, bool generatingBoards)
        {
            int rowPlaced = 0;
            for (int i = rows - 1; i >= 0; i--)
            {
                if (board[i, column] == -1)
                {
                    if (playerTurn == false)
                    {
                        board[i, column] = 0;
                        if (generatingBoards == false)
                        {
                            rootStart.currentBoard[i, column] = 0;
                            dgv_Board.Rows[i].Cells[column].Style.BackColor = Color.Red;
                        }
                        rowPlaced = i;
                        break;
                    }
                    else
                    {
                        board[i, column] = 1;
                        if (generatingBoards == false)
                        {
                            rootStart.currentBoard[i, column] = 1;
                            dgv_Board.Rows[i].Cells[column].Style.BackColor =
                            Color.Black; dgv_Board.Rows[i].Cells[column].Style.BackColor = Color.Black;
                        }
                        rowPlaced = i;
                        break;
                    }
                }
            }
            return rowPlaced;
        }

        #endregion

        #region GENERATE_FUTURE_BOARDS_HEURISTIC_AND_MINIMAX
        private void generateFutureBoards(Node root, int currDepth, int maxDepth, bool player)
        {
            currDepth++;
            if (root.childDepth >= maxDepth)
            {
                return;
            }
            else
            {
                for (int col = 0; col < columns; col++)
                {
                    if (root.currentBoard[0, col] == -1)
                    {
                        Node newChild = new Node(rows, columns, currDepth, col, player, connectR);
                        for (int colu = 0; colu < columns; colu++)
                        {
                            for (int row = 0; row < rows; row++)
                            {
                                newChild.currentBoard[row, colu] = root.currentBoard[row, colu];
                            }
                        }
                        addPlayerMove(col, newChild.currentBoard, player, true);
                        root.childList.Add(newChild);
                    }
                }
                foreach (Node child in root.childList)
                {
                    generateFutureBoards(child, currDepth, maxDepth, !player);
                }
            }
        }

        //go down to bottom of tree, make heuristic for bottom nodes.
        private Node makeHeuristic(Node rootNode, int depth)
        {
            if (rootNode.childDepth == (depth - 1))
            {
                //we are at root of this sub-tree
                //Max the child nodes here
                Node maxNode = rootNode;
                foreach (Node child in rootNode.childList)
                {
                    maxNode = child;
                    child.generateHeuristic();
                }
                return maxNode;
            }
            else
            {
                foreach (Node child in rootNode.childList)
                {
                    makeHeuristic(child, depth);
                }
                return rootNode;
            }
        }

        private Node miniMax(Node rootNode, int depth, bool maxOrMin) //return column to play
        {
            if (rootNode.childDepth == (depth - 1)) //bottom of tree
            {
                //do min of terminal nodes
                Node minChild = rootNode;
                foreach (Node child in rootNode.childList)
                {
                    if (minChild.heuristic == -100)
                    {
                        minChild = child;
                    }
                    else
                    {
                        if (minChild.heuristic < child.heuristic)
                        {
                            minChild = child;
                        }
                    }
                }
                return minChild;
            }
            else
            {
                List<Node> childNodes = new List<Node>();

                foreach (Node child in rootNode.childList)
                {
                    childNodes.Add(miniMax(child, depth, !maxOrMin));
                }

                if (maxOrMin)
                {
                    Node maxNode = childNodes[0];
                    foreach (Node max in childNodes)
                    {
                        if (maxNode.heuristic > max.heuristic)
                        {
                            maxNode = max;
                        }
                    }
                    return maxNode;
                }
                else
                {
                    Node minNode = childNodes[0];
                    foreach (Node min in childNodes)
                    {
                        if (min.heuristic < minNode.heuristic)
                        {
                            minNode = min;
                        }
                    }
                    return minNode;
                }
            }
        }

        #endregion

        #region CHECK_BOXES
        private void cb_PlayerVsAI_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_PlayerVsAI.Checked)
            {
                cb_AIFirst.Enabled = true;
            }
            else
            {
                cb_AIFirst.Enabled = false;
            }
        }

        private void cb_AIFirst_CheckedChanged(object sender, EventArgs e)
        {
            int AIPlacement = 0;
            playerTurn = false;
            rootStart = new Node(rows, columns, 0, 0, playerTurn, connectR);
            for (int rowCount = 0; rowCount < rows; rowCount++)
            {
                for (int columnCount = 0; columnCount < columns; columnCount++)
                {
                    boardState[rowCount, columnCount] = -1;
                    rootStart.currentBoard[rowCount, columnCount] = -1;
                }
            }

            if (rootStart.childList.Count != 0)
            {
                rootStart.childList.Clear();
            }
            generateFutureBoards(rootStart, 0, depth, playerTurn);
            makeHeuristic(rootStart, depth);
            AIPlacement = miniMax(rootStart, depth, true).columnPlayed;
            int AIRowPlayed = addPlayerMove(AIPlacement, this.boardState, playerTurn, false);
            cb_AIFirst.Enabled = false;
            cb_PlayerVsAI.Enabled = false;
        }
        #endregion

    }
}
