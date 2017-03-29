using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign2_WilliamStory
{
    class Node
    {
        public List<Node> childList {get; set;}
        public string behavior {get; set;}
        public string response {get; set;}

        public Node(string nBehavior, string nResponse)
        {
            childList = new List<Node>();
            behavior = nBehavior;
            response = nResponse;
        }

    }
}
