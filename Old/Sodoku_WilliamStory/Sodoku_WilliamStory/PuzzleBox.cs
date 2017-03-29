using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Sodoku_WilliamStory
{
    public partial class PuzzleBox : Form
    {
        private int[,] sodokuPuzzle;
        private List<int> currentNonet;
        private int placedNumbers = 0;

        private Thread solveThread, popThread;

        public PuzzleBox()
        {
            InitializeComponent();
            loadPuzzle();
        }

        #region LOADING PUZZLE / UI UPDATING
        private void loadPuzzle()
        {
            sodokuPuzzle = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sodokuPuzzle[i, j] = 0;
                }
            }

            OpenFileDialog oFD = new OpenFileDialog();
            oFD.Filter = "Text Files (.txt)|*.txt";
            oFD.FilterIndex = 1;
            oFD.ShowDialog();

            string currLine = "";
            string[,] initialPuzzle = new string[9, 9];
            char delim = ',';
            int currentRow = 0; //Need to keep track of what row we're on while parsing the puzzle.

            StreamReader puzzleReader = new StreamReader(oFD.FileName);
            try
            {
                while ((currLine = puzzleReader.ReadLine()) != null)
                {
                    string[] row = currLine.Split(delim);
                    for (int i = 0; i < row.Length; i++)
                    {
                        if (!(row[i].Equals("")))
                        {
                            int number = 0;
                            Int32.TryParse(row[i], out number);
                            sodokuPuzzle[currentRow, i] = number;
                        }
                    }
                    currentRow++;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Application.Exit();
            }
            puzzleReader.Close();
            populateDGV();
        }
        private void populateDGV()
        {
            //Set up row headers so it's easy to see the row numbers
            for (int i = 0; i < 9; i++)
            {
                ViewPuzzle.Rows.Add();
                ViewPuzzle.Rows[i].HeaderCell.Value = "R" + (1+i);
                ViewPuzzle.Rows[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //Populate the rows
            popThread = new Thread(() =>
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int k = 0; k < 9; k++)
                        {
                            ViewPuzzle.Rows[i].Cells[k].Value = sodokuPuzzle[i, k];
                            ViewPuzzle.Invoke((MethodInvoker)delegate
                            {
                                if (sodokuPuzzle[i, k] != 0)
                                {
                                    ViewPuzzle.Rows[i].Cells[k].Style.BackColor = Color.Aquamarine;
                                }
                            });
                        }
                    }
                    popThread.Join();
                }
            );
            popThread.Start();
        }
        #endregion

        #region SOLVE PUZZLE

        private void b_Solve_Click(object sender, EventArgs e)
        {
            solvePuzzle();
        }

        
        private void solvePuzzle()
        {
            bool giveUp = false; //Quit if program can't figure out the puzzle.
            int passes = 0; //Current amount of passes without placing a number.
            List<int> possibleAnswers; //Possible answers for this current box.
            solveThread = new Thread(() =>
                {
                    while (!giveUp)
                    {
                        //Step through each box
                        for (int i = 0; i < 9; i++)
                        {
                            for (int k = 0; k < 9; k++)
                            {
                                getPossibleAnswers(k, i, out possibleAnswers);
                                if (possibleAnswers.Count == 1 && sodokuPuzzle[k, i] == 0)
                                {
                                    ViewPuzzle.Invoke((MethodInvoker)delegate
                                        {
                                            ViewPuzzle.Rows[k].Cells[i].Value = possibleAnswers[0];
                                            ViewPuzzle.Rows[k].Cells[i].Style.BackColor = Color.Green;
                                            sodokuPuzzle[k, i] = possibleAnswers[0];
                                            placedNumbers++;
                                        }
                                    );
                                    Thread.Sleep(150);
                                }
                            }
                        }
                        passes++;
                        if (passes == 30)
                        {
                            giveUp = true;
                            MessageBox.Show("Solved to the best of my ability!" +"\n"
                                +"Placed numbers: " + placedNumbers);
                        }
                            
                    }
                    solveThread.Join();
                }
            );
            solveThread.Start();
        }

        //First method: Look to the left/right/up/down and see if there is an answer.
        private void getPossibleAnswers(int row, int column, out List<int> possAnswers)
        {
            possAnswers = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                possAnswers.Add(i + 1);
            }

            if (sodokuPuzzle[row, column] != 0)//This box is already filled in, so no need to solve it.
            {
                return;
            }

            //Build the current nonet square we're in.
            currentNonet = new List<int>();
            if (column % 3 == 0 && row % 3 == 0)
            {
                currentNonet.Clear();
                //In the upper left corner of the nonet square
                //Now it's just a matter of looping a 3x3 grid
                for (int i = 0; i < 3; i++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        int boxValue = sodokuPuzzle[i + row, k + column];
                        if (boxValue != 0)
                        {
                            currentNonet.Add(boxValue);
                        }
                    }
                }
            }
            else
            {
                currentNonet.Clear();
                //Find out where the upper left hand corner of this square is.
                int startRow = row - (row % 3); //upper corner.
                int startColumn = column - (column % 3); //most left column.

                //Go through the nonet square
                for (int i = 0; i < 3; i++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        int boxValue = sodokuPuzzle[i + startRow, k + startColumn];
                        if (boxValue != 0)
                        {
                            currentNonet.Add(boxValue);
                        }
                    }
                }
            }

            //Remove nonent from possible answers.
            for (int i = 0; i < currentNonet.Count; i++)
            {
                possAnswers.Remove(currentNonet[i]);
            }

            //Let's look at the row we're on first
            for (int i = 0; i < 9; i++)
            {
                int boxValue = sodokuPuzzle[row, i];
                if (boxValue != 0)
                {
                    possAnswers.Remove(boxValue);
                }
            }
            //Now lets take a look at what column we're on.
            for (int i = 0; i < 9; i++)
            {
                int boxValue = sodokuPuzzle[i, column];
                if (boxValue != 0)
                {
                    possAnswers.Remove(boxValue);
                }
            }
            
        }

        #endregion

        private void ViewPuzzle_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

    }
}
