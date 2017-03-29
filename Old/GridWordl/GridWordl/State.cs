using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridWordl
{
    class State
    {
        public enum move
        {
            UP = 0,
            DOWN = 1,
            LEFT = 2,
            RIGHT = 3,
        };

        public double up_Weight, down_Weight;
        public double left_Weight, right_Weight;
        public double[] moveWeights; //In order: Up, Down, Left, Right.

        public move directionTaken;

        public bool isTerminalState = false;
        public int rowIndex;
        public int columnIndex;
        public double rewardValue = 0.0;

        public State()
        {

        }

        public move exploit_Move()
        {
            double highestMove = moveWeights[0];
            move direction = 0;
            for (int i = 1; i < moveWeights.Length; i++)
            {
                if (moveWeights[i] > highestMove)
                {
                    highestMove = moveWeights[i];
                    direction = (move)i;
                }
            }
            directionTaken = direction;
            return direction;
        }

        public move explore_Move(Random rand)
        {
            move direction = 0;
            double roll = rand.NextDouble() * (1.0 - 0.0) + 0.0;
            if (roll >= 0.75)
            {
                direction = move.UP;
            }
            else if (roll >= .50)
            {
                direction = move.DOWN;
            }
            else if (roll >= .25)
            {
                direction = move.LEFT;
            }
            else
            {
                direction = move.RIGHT;
            }
            directionTaken = direction;
            return direction;

        }

        public void initWeights(Random rand)
        {
            moveWeights = new double[4];
            
            for (int i = 0; i < 4; i++)
            {
                double init_Weight = rand.NextDouble() * (0.09 - 0.01) + 0.01;
                moveWeights[i] = init_Weight;
            }
        }

        public string getWeights()
        {
            StringBuilder weights = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                if (weights.Length > 0)
                    weights.Append(",");
                weights.Append(moveWeights[i] + "");
            }
            return weights.ToString();
        }

        public void loadWeigths(string line)
        {
            moveWeights = new double[4];
            List<string> weights = line.Split(',').ToList();
            for (int i = 0; i < weights.Count; i++)
            {
                double weight = double.Parse(weights[i]);
                moveWeights[i] = weight;
            }
        }
    }
}
