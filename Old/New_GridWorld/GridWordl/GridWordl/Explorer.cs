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

        public double learnRate = 0.01; //
        public double gamma = 0.1; //gamma
        public double epsilon = .99; //
        public double decay_Rate = .000001; //How fast to switch from explore to exploit.
        public double reward = 0.0;
        public double lambda = 0.3;
        public double delta = 0.0;

        public State oldState;
        public State newState;

        public State[,] states; //Q table
        public State[,] eStates; //E(s,a) table
        public List<State> stepsTaken; //Trace table

        public State.move oldAction;
        public State.move newAction;

        public int startX, startY;

        public bool inTerminalState = false;
        public State.move directionMoved;

        public string fileName = "Q_TableSave.txt";

        public Explorer(int[,] boardState)
        {
            states = new State[20, 20];
            eStates = new State[20, 20];
            Random rand = new Random();
            stepsTaken = new List<State>();
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
                    currState = boardState[this.startX, this.startY];
                    if (currState == 0 || currState == 1)
                    {
                        inTerminalState = true;
                    }
                    else
                        inTerminalState = false;
                }
            }
            oldState = states[startX, startY];
            this.boardState = boardState;
        }

        public void resetStart(Random rand)
        {
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
                    currState = boardState[this.startX, this.startY];
                    if (currState == 0 || currState == 1)
                    {
                        inTerminalState = true;
                    }
                    else
                        inTerminalState = false;
                }
            }
            oldState = states[startX, startY];
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
            oldState = states[this.startX, this.startY];

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

        public State exploreMove(Random rand)
        {
            //current action, state already initialized by this point.
            //take action A here.
            int[] newStateLoc = getMove(this.oldState, oldAction);
            stepsTaken.Add(oldState);
            //get S prime here.
            newState = getNextState(newStateLoc[0], newStateLoc[1]);
            //Observe the reward of S prime.
            reward = getReward(newStateLoc[0], newStateLoc[1]);
            //Now it's time to explore
            //Get the A prime from S prime.
            newAction = newState.explore_Move(rand);
            //get delta here
            delta = reward + gamma * newState.moveWeights[(int)newAction] - oldState.moveWeights[(int)oldAction];
            //set eligibilty here
            eStates[oldState.rowIndex, oldState.columnIndex].moveWeights[(int)oldAction] += 1;
            //For all S, A update
            for (int rows = 0; rows < 20; rows++)
            {
                for (int cols = 0; cols < 20; cols++)
                {
                    states[rows, cols].moveWeights[(int)oldAction] = states[rows, cols].moveWeights[(int)oldAction]
                        + learnRate * delta * eStates[rows, cols].moveWeights[(int)oldAction];
                    eStates[rows, cols].moveWeights[(int)oldAction] = lambda * gamma * eStates[rows, cols].moveWeights[(int)oldAction];
                }
            }
            //Update s to s' and a to a' here.
            oldAction = newAction;
            oldState = newState;
            return newState;
        }

        public State exploitMove()
        {
            //current action, state already initialized by this point.
            //take action A here.
            int[] newStateLoc = getMove(this.oldState, oldAction);
            stepsTaken.Add(oldState);
            //get S prime here.
            newState = getNextState(newStateLoc[0], newStateLoc[1]);
            //Observe the reward of S prime.
            reward = getReward(newStateLoc[0], newStateLoc[1]);
            //Now it's time exploit.
            //Get the A prime from S prime.
            newAction = newState.exploit_Move();
            //get delta here
            delta = reward + gamma * newState.moveWeights[(int)newAction] - oldState.moveWeights[(int)oldAction];
            //set eligibilty here
            eStates[oldState.rowIndex, oldState.columnIndex].moveWeights[(int)oldAction] += 1;
            //For all S, A update
            for (int rows = 0; rows < 20; rows++)
            {
                for (int cols = 0; cols < 20; cols++)
                {
                    states[rows, cols].moveWeights[(int)oldAction] = states[rows, cols].moveWeights[(int)oldAction]
                        + learnRate * delta * eStates[rows, cols].moveWeights[(int)oldAction];
                    eStates[rows, cols].moveWeights[(int)oldAction] = lambda* gamma * eStates[rows, cols].moveWeights[(int)oldAction];
                }
            }
            //Update s to s' and a to a' here.
            oldAction = newAction;
            oldState = newState;
            return newState;
        }

        public int[] getMove(State currState, State.move moveDirection)
        {
            int[] moveCoordinates = new int[2];
            switch (moveDirection)
            {
                case State.move.UP:
                    moveCoordinates = new int[] {currState.rowIndex - 1, currState.columnIndex};
                    return moveCoordinates;
                case State.move.DOWN:
                    moveCoordinates = new int[] {currState.rowIndex + 1, currState.columnIndex};
                    return moveCoordinates;
                case State.move.LEFT:
                    moveCoordinates = new int[] {currState.rowIndex, currState.columnIndex - 1};
                    return moveCoordinates;
                case State.move.RIGHT:
                    moveCoordinates = new int[] {currState.rowIndex, currState.columnIndex + 1};
                    return moveCoordinates;
                default:
                    moveCoordinates = new int[] { 0, 0 };
                    return moveCoordinates;
            }
        }
        public bool checkTerminal(int row, int col)
        {
            if (row < 0 || row > 19 || col < 0 || col > 19)
            {
                return true;
            }
            else if (boardState[row, col] == 0)
            {
                return true;
            }
            else if (boardState[row, col] == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getReward(int row, int col)
        {
            if (row < 0 || row > 19 || col > 19 || col < 0)
            {
                return -5;
            }
            else if (boardState[row, col] == 0)
            {
                return -5;
            }
            else if (boardState[row, col] == 1)
            {
                return 50;
            }
            else
            {
                return 1;
            }
        }
        public State getNextState(int row, int col)
        {
            State next;
            if (row > 19 || row < 0 || col > 19 || col < 0)
            {
                next = new State();
                next.rewardValue = -5;
                next.isTerminalState = true;
                next.moveWeights = new double[] { -5.0, -5.0, -5.0, -5.0};
            }
            else
            {
                next = states[row, col];
            }

            return next;
        }
    }
}
