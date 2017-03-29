using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_WilliamStory
{
    class Layer
    {
        public Node[] layerNodeArray;
        public Node biasNode;
        public int nodeCount;
        public Layer childLayer;
        bool isOutPutLayer;

        public Layer(int nodeCount, bool isOutPutLayer)
        {
            this.isOutPutLayer = isOutPutLayer;
            layerNodeArray = new Node[nodeCount];
            this.nodeCount = nodeCount;
            biasNode = null;
        }

        public void generateLayer(Layer childLayer)
        {
            Random rand = new Random();
            int childNodeCount = childLayer.nodeCount;

            for (int i = 0; i < layerNodeArray.Length; i++)
            {
                Node layer_Node = new Node();
                layer_Node.weights = new double[childNodeCount];
                for (int k = 0; k < layer_Node.weights.Length; k++)
                {
                    double initWeight = rand.NextDouble() * (.00009 - .00001) + .00001;
                    layer_Node.weights[k] = initWeight;
                }
                layerNodeArray[i] = layer_Node;
            }

            if (childLayer.isOutPutLayer)
            {
                for(int i = 0; i < childLayer.nodeCount; i++){
                    
                    Node layer_Node = new Node();
                    layer_Node.weights = new double[childLayer.nodeCount];
                    for (int k = 0; k < layer_Node.weights.Length; k++)
                    {
                        double initWeight = rand.NextDouble() * (.009 - .001) + .001;
                        layer_Node.weights[k] = initWeight;
                    }
                    childLayer.layerNodeArray[i] = layer_Node;
                }
            }
        }

        public void setBiasNode(Node biasNode)
        {
            if (biasNode.weights.Length != this.nodeCount)
            {
                Console.Out.WriteLine("Error 1");
            }
            this.biasNode = biasNode;
            foreach (Node layer_Node in this.layerNodeArray)
            {
                layer_Node.parentList.Add(biasNode);
                this.biasNode.childList.Add(layer_Node);
            }
        }
    }
}
