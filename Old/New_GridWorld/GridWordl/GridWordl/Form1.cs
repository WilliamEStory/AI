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

namespace GridWordl
{
    public partial class Form1 : Form
    {
        int[,] boardState = new int[20, 20];
        Explorer explore;
        Explorer oldExplore;

        Thread exploreThread;

        public Form1()
        {
            InitializeComponent();
            init_GUI_Board();
            this.Shown += Form1_Shown;
            this.FormClosing += Form1_FormClosing;
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //explore.saveQ_Table();
        }

        void Form1_Shown(object sender, EventArgs e)
        {
            //explore.initializeQ_Table();
            //explore.loadQ_Table();
            //dgv_World.Rows[explore.startX].Cells[explore.startY].Style.BackColor = Color.Green;
            explore = new Explorer(boardState);
            explore.loadQ_Table();
        }

        

        private void init_GUI_Board()
        {
            //Load the board here
            StreamReader reader;
            string saveFile = @"C:\Users\William_2\Documents\visual studio 2012\Projects\GridWordl\GridWordl\world.txt";
            reader = new StreamReader(saveFile);
            string currentLine = reader.ReadLine();
            int cellValue = 0;
            for (int rows = 0; rows < 20; rows++)
            {
                List<string> world = currentLine.Split(',').ToList();
                DataGridViewRow row = new DataGridViewRow();
                row.HeaderCell.Value = "Row " + rows;
                dgv_World.Rows.Add(row);
                for (int cols = 0; cols < world.Count - 1; cols++)
                {
                    if (world[cols] == "0")
                    {
                        dgv_World.Rows[rows].Cells[cols].Style.BackColor = Color.Black;
                        boardState[rows, cols] = 0;
                    }
                    else if (world[cols] == "2")
                    {
                        dgv_World.Rows[rows].Cells[cols].Style.BackColor = Color.White;
                        boardState[rows, cols] = 2;
                    }
                    else
                    {
                        dgv_World.Rows[rows].Cells[cols].Style.BackColor = Color.Blue;
                        boardState[rows, cols] = 1;
                    }
                    dgv_World.Rows[rows].Cells[cols].Value = cellValue++ + "";
                }
                currentLine = reader.ReadLine();
            }
        }

        private void reset_GUI()
        {
            int stateNum = 0;

            for (int rows = 0; rows < 20; rows++)
            {
                for (int cols = 0; cols < 20 - 1; cols++)
                {
                    if (boardState[rows, cols] == 0)
                    {
                        dgv_World.Rows[rows].Cells[cols].Style.BackColor = Color.Black;
                        
                    }
                    else if (boardState[rows, cols] == 2)
                    {
                        dgv_World.Rows[rows].Cells[cols].Style.BackColor = Color.White;
                        
                    }
                    else
                    {
                        dgv_World.Rows[rows].Cells[cols].Style.BackColor = Color.Blue;
                    }
                    dgv_World.Rows[rows].Cells[cols].Value = stateNum++;
                }
            }
        }

        private void dgv_World_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.ColumnIndex > dgv_World.Columns.Count || e.RowIndex > dgv_World.Rows.Count)
            //{
            //    return;
            //}
            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //{
            //    string saveFile = @"C:\Users\William_2\Documents\visual studio 2012\Projects\GridWordl\GridWordl\world.txt";
            //    StreamWriter writer = new StreamWriter(saveFile);

            //    List<string> savedWorld = new List<string>();
            //    foreach (DataGridViewRow row in dgv_World.Rows)
            //    {
            //        string save = "";
            //        foreach (DataGridViewColumn col in dgv_World.Columns)
            //        {
            //            if (dgv_World.Rows[row.Index].Cells[col.Index].Style.BackColor == Color.Black)
            //            {
            //                save += "0,";
            //            }
            //            else if (dgv_World.Rows[row.Index].Cells[col.Index].Style.BackColor == Color.Blue)
            //            {
            //                save += "1,";
            //            }
            //            else
            //            {
            //                save += "2,";
            //            }
            //        }
            //        savedWorld.Add(save);
            //    }
            //    foreach (string saveString in savedWorld)
            //    {
            //        writer.WriteLine(saveString);
            //    }
            //    writer.Close();
            //    Application.Exit();
            //}

            //if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            //{
            //    dgv_World.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Blue;
            //    return;
            //}
            //else
            //{
            //    dgv_World.Invoke(new MethodInvoker(() =>
            //    {
            //        dgv_World.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Black;
            //    }));
            //}
        }

        private void b_Start_Click(object sender, EventArgs e)
        {
            DateTime end = DateTime.UtcNow.AddHours(7.5);
            Color green = Color.FromArgb(0, 100, 0);
            int epochs = 0;
            Random rand = new Random();
            exploreThread = new Thread(() =>
           {
               while(explore.epsilon >= .05)
               {
                   //run 100 times before saving QSA to file 
                   //to reduce I/O usage.
                   for (int i = 0; i < 100; i++)
                   {
                       explore.reset_ETable();
                       State currState = explore.oldState;
                       explore.oldAction = currState.getInitAction(rand);
                       State.move currAction = explore.oldAction;
                       dgv_World.Invoke((MethodInvoker)delegate
                       {
                           dgv_World.Rows[currState.rowIndex].Cells[currState.columnIndex].Style.BackColor = Color.Green;
                       });
                       do
                       {
                           double exploreChance = rand.NextDouble() * (99.999999 - 0.000) + 0.000;
                           if (exploreChance <= explore.epsilon * 100)
                           {
                               explore.exploreMove(rand);
                               
                           }
                           else
                           {
                               explore.exploitMove();
                           }
                           if (explore.checkTerminal(explore.newState.rowIndex, explore.newState.columnIndex))
                           {
                               explore.inTerminalState = true;
                           }
                           currState = explore.newState;
                           currAction = explore.newAction;
                           dgv_World.Invoke((MethodInvoker)delegate
                           {
                               dgv_World.Rows[currState.rowIndex].Cells[currState.columnIndex].Style.BackColor = Color.LightGreen;
                               string move = "";
                               switch (currAction)
                               {
                                   case State.move.UP:
                                       move = "U";
                                       break;
                                   case State.move.DOWN:
                                       move = "D";
                                       break;
                                   case State.move.LEFT:
                                       move = "L";
                                       break;
                                   case State.move.RIGHT:
                                       move = "R";
                                       break;
                               }
                               dgv_World.Rows[currState.rowIndex].Cells[currState.columnIndex].Value = move;
                           });

                           if (explore.inTerminalState)
                           {
                               dgv_World.Invoke((MethodInvoker)delegate
                               {
                                   dgv_World.Rows[currState.rowIndex].Cells[currState.columnIndex].Style.BackColor = Color.Red;
                               });
                           }
                           Thread.Sleep(150);
                       } while (explore.oldState.isTerminalState == false);

                       
                       explore.epsilon = explore.epsilon - explore.decay_Rate;
                       if (explore.epsilon <= .10)
                       {
                           explore.epsilon = .10;
                       }
                       reset_GUI();
                       explore.stepsTaken.Clear();
                       explore.resetStart(rand);
                   }
                   explore.saveQ_Table();
                   if (DateTime.UtcNow > end)
                   {
                       break;
                   }
               }

               Application.Exit();
               exploreThread.Join();

           });
            exploreThread.Start();
            //string first = "Q_TableSave.txt";
            //string second = "Q_Save10.txt";
            //string[] linesA = File.ReadAllLines(first);
            //string[] linesb = File.ReadAllLines(second);
            //int count = 0;

            //for (int i = 0; i < linesA.Length; i++)
            //{
            //    if (linesA[i] != linesb[i])
            //    {
            //        count++;
            //    }
            //}
            //Console.Out.WriteLine(count);
        }
    }
}
