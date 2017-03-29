using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GridWordl
{
    class Explorer
    {
        public int[,] boardState = new int[20, 20];

        public double learnRate = 0.1; //
        public double discount = 0.4; //gamma
        public double epsilon = .95999; //
        public double decay_Rate = .00005; //How fast to switch from explore to exploit.
        public double reward = 1.0;
        public double lambda = 0.9;
        public double delta = 0.0;

        public State current_State;
        public State[,] states; //Q table
        public State[,] eStates; //E(s,a) table
        public List<State> stepsTaken; //Trace table

        public int startX, startY;

        public bool inTerminalState = false;
        public State.move directionMoved;

        public string fileName = "Q_TableSave.txt";

        public Explorer(int[,] boardState)
        {
            states = new State[20, 20];
            eStates = new State[20, 20];
            Random rand = new Random();
            this.startX = rand.Next(0, 19);
            this.startY = rand.Next(0, 19);
            int currState = boardState[startX, startY];
            if (currState == 0 || currState == 1)
            {
                inTerminalState = true;
            }
            if (inTerminalState)
            {
                while (inTerminalState)
                {
                    this.startX = rand.Next(0, 19);
                    this.startY = rand.Next(0, 19);
                    currState = boardState[startX, startY];
                    if (currState == 0 || currState == 1)
                    {
                        inTerminalState = true;
                    }
                    else
                        inTerminalState = false;
                }
            }
            current_State = states[startX, startY];
            stepsTaken = new List<State>();
            this.boardState = boardState;
        }

        public void reset_ETable()
        {
            for(int rows = 0; rows < 20; rows++)
            {
                for (int cols = 0; cols < 20; cols++)
                {
                    eStates[rows,cols] = new State();
                    eStates[rows, cols].rewardValue = 0;
                    eStates[rows,cols].moveWeights = new double[] { 0.0, 0.0, 0.0, 0.0 };
                }
                
            }
        }

        public void loadQ_Table()
        {
            StreamReader reader = new StreamReader(fileName);
            string saved_Epsilon = reader.ReadLine();
            epsilon = double.Parse(saved_Epsilon);

            for (int rows = 0; rows < 20; rows++)
            {
                for (int cols = 0; cols < 20; cols++)
                {
                    string currentLine = reader.ReadLine();

                    State state = new State();
                    state.loadWeigths(currentLine);
                    state.rowIndex = rows;
                    state.columnIndex = cols;

                    if (boardState[rows, cols] == 0
                        || boardState[rows, cols] == 1)
                    {
                        state.isTerminalState = true;
                    }

                    if (state.isTerminalState)
                    {
                        Console.WriteLine("in here");
                    }

                    states[rows, cols] = state;

                    currentLine = reader.ReadLine();
                }

               
            }
            current_State = states[this.startX, this.startY];

            reader.Close();
        }

        public void saveQ_Table()
        {
            
            StreamWriter writer = new StreamWriter(fileName);
            writer.WriteLine(epsilon + "");

            for (int rows = 0; rows < 20; rows++)
            {
                for (int cols = 0; cols < 20; cols++)
                {
                    State currState = states[rows, cols];
                    string state_Weights = currState.getWeights();
                    writer.WriteLine(state_Weights + "\n");
                }
            }
            writer.Close();
        }

        public void initializeQ_Table()
        {
            Random rand = new Random();

            for (int rows = 0; rows < 20; rows++)
            {
                for (int cols = 0; cols < 20; cols++)
                {
                    State newState = new State();
                    newState.initWeights(rand);
                    states[rows, cols] = newState;
                }
            }
        }

        public State explore(Random rand)
        {
            stepsTaken.Add(current_State);
            State.move moveDir = current_State.explore_Move(rand);
            states[current_State.rowIndex, current_State.columnIndex].directionTaken = moveDir;
            current_State.directionTaken = moveDir;
            directionMoved = moveDir;

            if (moveDir == State.move.UP)
            {
                bool terminal = checkForTerminal(current_State.rowIndex - 1, current_State.columnIndex);
                if (terminal)
                {
                    current_State.directionTaken = moveDir;
                    return current_State;
                }
                else
                {
                    current_State = states[current_State.rowIndex - 1, current_State.columnIndex];
                    current_State.directionTaken = moveDir;
                }
            }
            else if (moveDir == State.move.DOWN)
            {
                bool terminal = checkForTerminal(current_State.rowIndex + 1, current_State.columnIndex);
                if (terminal)
                {
                    current_State.directionTaken = moveDir;
                    return current_State;
                }
                else
                {
                    current_State = states[current_State.rowIndex + 1, current_State.columnIndex];
                    current_State.directionTaken = moveDir;
                }
            }
            else if (moveDir == State.move.LEFT)
            {
                bool terminal = checkForTerminal(current_State.rowIndex, current_State.columnIndex - 1);
                if (terminal)
                {
                    current_State.directionTaken = moveDir;
                    return current_State;
                }
                else
                {
                    current_State = states[current_State.rowIndex, current_State.columnIndex - 1];
                    current_State.directionTaken = moveDir;
                }
            }
            else
            {
                bool terminal = checkForTerminal(current_State.rowIndex, current_State.columnIndex + 1);
                if (terminal)
                {
                    current_State.directionTaken = moveDir;
                    return current_State;
                }
                else
                {
                    current_State = states[current_State.rowIndex, current_State.columnIndex + 1];
                    current_State.directionTaken = moveDir;
                }
            }
            current_State.directionTaken = moveDir;
            return current_State;
        }

        public State exploit()
        {
            stepsTaken.Add(current_State);
            State.move moveDir = current_State.exploit_Move();
            states[current_State.rowIndex, current_State.columnIndex].directionTaken = moveDir;
            directionMoved = moveDir;
            current_State.directionTaken = moveDir;
            if (moveDir == State.move.UP)
            {
                bool terminal = checkForTerminal(current_State.rowIndex, current_State.columnIndex - 1);
                if (terminal)
                {
                    return current_State;
                }
                else
                {
                    current_State = states[current_State.rowIndex, current_State.columnIndex - 1];
                }
            }
            else if (moveDir == State.move.DOWN)
            {
                bool terminal = checkForTerminal(current_State.rowIndex, current_State.columnIndex + 1);
                if (terminal)
                {
                    return current_State;
                }
                else
                {
                    current_State = states[current_State.rowIndex, current_State.columnIndex + 1];
                }
            }
            else if (moveDir == State.move.LEFT)
            {
                bool terminal = checkForTerminal(current_State.rowIndex - 1, current_State.columnIndex);
                if (terminal)
                {
                    return current_State;
                }
                else
                {
                    current_State = states[current_State.rowIndex - 1, current_State.columnIndex];
                }
            }
            else
            {
                bool terminal = checkForTerminal(current_State.rowIndex + 1, current_State.columnIndex);
                if (terminal)
                {
                    return current_State;
                }
                else
                {
                    current_State = states[current_State.rowIndex + 1, current_State.columnIndex];
                }
            }
            return current_State;
        }

        public bool checkForTerminal(int row, int col)
        {
            if (row < 0 || row > 19 || col < 0 || col > 19) //hit an out of bounds state
            {
                current_State.isTerminalState = true;

                //State borderState = new State();
                //borderState.moveWeights = new double[] { -1.0, -1.0, -1.0, -1.0 };
                //borderState.rowIndex = row;
                //borderState.columnIndex = col;
                //stepsTaken.Add(borderState);
                //borderState.isTerminalState = true;
                //current_State = borderState;

                //reward = -1;
                //updateE_StateActionTable();

                return true;
            }
            else if (boardState[row, col] == 0)
            {
                current_State.isTerminalState = true;

                //State borderState = new State();
                //borderState.moveWeights = new double[] { -1.0, -1.0, -1.0, -1.0 };
                //borderState.rowIndex = row;
                //borderState.columnIndex = col;
                //stepsTaken.Add(borderState);
                //borderState.isTerminalState = true;
                //current_State = borderState;

                //reward = -1;
                //updateE_StateActionTable();
                
                return true;
            }
            else if (boardState[row, col] == 1)
            {
                current_State.isTerminalState = true;

                //State borderState = new State();
                //borderState.moveWeights = new double[] { 1.0,1.0, 1.0, 1.0 };
                //borderState.rowIndex = row;
                //borderState.columnIndex = col;
                //stepsTaken.Add(borderState);
                //borderState.isTerminalState = true;
                //current_State = borderState;

                //reward = 1;
                //updateE_StateActionTable();

                return true;
            }
            else
            {
                reward = 1;
                //updateE_StateActionTable();

                return false;
            }
        }

        public void updateE_StateActionTable()
        {
            for (int i = stepsTaken.Count - 1; i >= 0; i--)
            {
                stepsTaken[i].rewardValue = reward;
                reward = reward - (reward * discount);
            }
        }

        public void updateQ_StateActionTable()
        {
            for (int i = stepsTaken.Count - 1; i > 0; i--)
            {
                State state = stepsTaken[i];
                State previousState = stepsTaken[i - 1];
                //State Q_Current = states[state.rowIndex, state.columnIndex];
                //State Q_Previous = states[previousState.rowIndex, previousState.columnIndex];

                switch (previousState.directionTaken)
                {
                    case State.move.UP:
                        previousState.moveWeights[0] =
                            previousState.moveWeights[0]
                            + learnRate * (previousState.rewardValue +
                            discount * state.moveWeights[0] - previousState.moveWeights[0]);
                        break;
                    case State.move.DOWN:
                        previousState.moveWeights[1] =
                            previousState.moveWeights[1]
                            + learnRate * (previousState.rewardValue + 
                            discount*state.moveWeights[1] - previousState.moveWeights[1]);
                        break;
                    case State.move.LEFT:
                        previousState.moveWeights[2] =
                            previousState.moveWeights[2]
                            + learnRate * (previousState.rewardValue +
                            discount * state.moveWeights[2] - previousState.moveWeights[2]);
                        break;
                    case State.move.RIGHT:
                        previousState.moveWeights[3] =
                            previousState.moveWeights[3]
                            + learnRate * (previousState.rewardValue +
                            discount * state.moveWeights[3] - previousState.moveWeights[3]);
                        break;
                    default:
                        break;
                }
                for(int weight = 0; weight < previousState.moveWeights.Length; weight++)
                {
                    if (previousState.moveWeights[weight] <= -1.0)
                    {
                        previousState.moveWeights[weight] = -1.0;
                    }
                    if (previousState.moveWeights[weight] >= 1.0)
                    {
                        previousState.moveWeights[weight] = 1.0;
                    }
                }
                states[previousState.rowIndex, previousState.columnIndex] = previousState;
            }
            stepsTaken.Clear();
        }

        public void setRewardValue(State currentState)
        {
            if (boardState[current_State.rowIndex, current_State.columnIndex] == 0)
            {
                //hit a wall
                reward = -1;
            }
            else if (boardState[current_State.rowIndex, current_State.columnIndex] == 1)
            {
                //goal state
                reward = 1;
            }
            else
            {
                //just on a regular white space
                reward = 0;
            }
        }

        public void setDelta(State currentState, State oldState)
        {
            delta =  reward + (discount * (states[current_State.rowIndex, current_State.columnIndex].moveWeights[(int)currentState.directionTaken]
                - states[oldState.rowIndex, oldState.columnIndex].moveWeights[(int)oldState.directionTaken]));
        }

        public double updateETable(int rowIndex, int colIndex, int directionTaken)
        {
            return discount * lambda * eStates[rowIndex, colIndex].moveWeights[directionTaken];
        }

        public double updateQTable(int rowIndex, int colIndex, int directionTaken)
        {
            return states[rowIndex, colIndex].moveWeights[directionTaken] += learnRate * delta * eStates[rowIndex, colIndex].moveWeights[directionTaken];
        }
    }
}
