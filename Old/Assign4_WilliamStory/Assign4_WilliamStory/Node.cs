using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_WilliamStory
{
    class Node
    {
        public List<Node> childList;
        public List<Node> parentList;
        public double[] weights;
        public double inputError = 0.0;
        public double outputError = 0.0;
        public double inputSum = 0.0;
        public double output = 0.0;

        public double weight, value;

        public Node()
        {
            childList = new List<Node>();
            parentList = new List<Node>();

            this.value = value;
        }
    }
}
