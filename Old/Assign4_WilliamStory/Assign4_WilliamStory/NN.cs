using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_WilliamStory
{
    static class NN
    {
        public static double[][] weights { get; set; }

        public static List<Layer> layers;
        public static List<double> input;
        public static List<double> output;
        public static int guessResult;


        public static int initialInput;
        public static int hiddenLayerOneInput;
        public static int hiddenLayerTwoInput;
        public static int outPutLayer = 10;

        public static double learningRate = .01;

        //public NN(int initialInputSize)
        //{
        //    layers = new List<Layer>();
        //    input = new List<double>();
        //    output = new List<double>();
        //    this.initialInput = initialInputSize;
        //}

        public static void generateNN(int initialInput)
        {
            NN.initialInput = initialInput;
            hiddenLayerOneInput = 50;
            hiddenLayerTwoInput = 30;
            layers = new List<Layer>();

            Layer inputLayer = new Layer(initialInput, false);
            Layer hiddenOne = new Layer(hiddenLayerOneInput, false);
            Layer hiddenTwo = new Layer(hiddenLayerTwoInput, false);
            Layer output = new Layer(outPutLayer, true);

            layers.Add(inputLayer);
            layers.Add(hiddenOne);
            layers.Add(hiddenTwo);
            layers.Add(output);

            Random rand = new Random();

            //Link layers together
            for (int i = 0; i < layers.Count() - 1; i++)
            {
                layers[i].childLayer = layers[i + 1];
            }

            //Generate each layer in layer list
            for( int currLayer = 0; currLayer < layers.Count() - 1; currLayer++)
            {
                layers[currLayer].generateLayer(layers[currLayer + 1]);
            }

            //Link each node in layer to each node in child_layer
            for (int i = 0; i < layers.Count() - 1; i++)
            {
                foreach (Node nodeInLayer in layers[i].layerNodeArray)
                {
                    foreach (Node nodeInNextLayer in layers[i + 1].layerNodeArray)
                    {
                        nodeInLayer.childList.Add(nodeInNextLayer);
                        nodeInNextLayer.parentList.Add(nodeInLayer);
                    }
                }
            }

            //Set bias node for each layer here
            //The way I have it set up, the first layer does not have a bias in it.
            //Every layer after it, including the output layer, does have a bias in it.
            for(int i = 1; i < layers.Count(); i++)
            {
                Layer layer = layers[i];
                Node biasNode = new Node();
                biasNode.value = 1;
                biasNode.weights = new double[layer.nodeCount];
                for (int k = 0; k < biasNode.weights.Length; k++)
                {
                    double initWeight = rand.NextDouble() * (.009 - .001) + .001;
                    biasNode.weights[k] = initWeight;
                }
                layer.setBiasNode(biasNode);
            }

            Console.Out.WriteLine("Finished building neural network");
        }

        public static void forwardPropagate(List<int> input)
        {
            for (int i = 0; i < layers[0].layerNodeArray.Count(); i++)
            {
                layers[0].layerNodeArray[i].value = input[i];
                layers[0].layerNodeArray[i].output = input[i];
            }

            //Go through all the layers
            for (int k = 0; k < layers.Count() - 1; k++)
            {
                //for each node in this layer
                foreach (Node inputNode in layers[k].layerNodeArray)
                {
                    Node biasNode = layers[k + 1].biasNode;
                    //foreach weight in this current node
                    for (int inputWeight = 0; inputWeight < inputNode.weights.Length; inputWeight++)
                    {
                        double weight = 0.0;
                        weight = inputNode.weights[inputWeight];
                        inputNode.childList[inputWeight].inputSum += (weight * inputNode.output) + biasNode.weights[inputWeight];
                    }
                }

                //sigmoid function
                if (k == layers.Count() - 1)
                {
                    //don't squash output layer
                    foreach (Node childNode in layers[k + 1].layerNodeArray)
                    {
                        double childValue = childNode.inputSum;
                        childNode.output = childValue;
                    }
                }
                else
                {
                    foreach (Node childNode in layers[k + 1].layerNodeArray)
                    {
                        double childValue = childNode.inputSum;
                        childValue = sigmoidFunc(childValue);
                        childNode.output = childValue;
                    }
                }
            }
        }

        public static double sigmoidFunc(double childValue)
        {
            return (1 / (1 + Math.Exp(-childValue)));
        }

        public static double sigmoidDerivate_Func(double x)
        {
            return sigmoidFunc(x) * (1 - sigmoidFunc(x));
        }

        public static void backwardPropagate()
        {
            //go through all layers
            for (int i = layers.Count - 1; i > 0; i--)
            {
                foreach (Node node in layers[i].layerNodeArray)
                {
                    foreach (Node parentNode in node.parentList)
                    {
                        parentNode.inputError = node.outputError;
                    }
                }
                foreach (Node parentNode in layers[i - 1].layerNodeArray)
                {
                    parentNode.outputError = sigmoidDerivate_Func(parentNode.inputError) * NN.learningRate;
                    for(int k = 0; k < parentNode.weights.Length; k++)
                    {
                        parentNode.weights[k] += parentNode.outputError;
                    }
                }
            }
        }
    }
}
