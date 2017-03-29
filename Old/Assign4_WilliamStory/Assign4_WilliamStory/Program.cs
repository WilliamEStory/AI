using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Assign4_WilliamStory
{
    class Program
    {
        static List<string> testResults = new List<string>();
        static string fileSave = "save.csv";

        static void Main(string[] args)
        {
            StreamReader reader = new StreamReader("optdigits_train.txt");
            
            NN.generateNN(64);
            testResults.Add("% Correct Each 100 Generations");

            do
            {
                for (int runs = 0; runs < 100; runs++)
                {
                    do
                    {
                        string line = reader.ReadLine();
                        List<int> inputs = line.Split(',').Select(x => int.Parse(x)).ToList();
                        int targetValue = inputs.Last();

                        NN.forwardPropagate(inputs);

                        int outputvalue = 0;
                        int outputLayer_Index = NN.layers.Count - 1;
                        Node best_Guess_Node = NN.layers[outputLayer_Index].layerNodeArray[0];
                        Layer outputLayer = NN.layers[outputLayer_Index];

                        double errorTotal = 0.0;

                        //Console.WriteLine("____________________________________");
                        //Console.WriteLine("On line: {0}", lineTag);
                        //lineTag++;
                        for (int i = 0; i < outputLayer.nodeCount; i++)
                        {
                            if (outputLayer.layerNodeArray[i].value > best_Guess_Node.value)
                            {
                                best_Guess_Node = outputLayer.layerNodeArray[i];
                                outputvalue = i;
                            }
                            NN.layers[outputLayer_Index].layerNodeArray[i].inputError = (NN.sigmoidDerivate_Func(i - targetValue)) * NN.learningRate;

                            //Console.Out.WriteLine(outputLayer.layerNodeArray[i].output);
                        }

                        NN.backwardPropagate();
                    } while (!reader.EndOfStream);
                    reader.Close();
                    reader = new StreamReader("optdigits_train.txt");
                    if (runs % 10 == 0 && runs > 1)
                    {
                        testNN();
                    }
                }
                

            } while (Console.ReadLine() != ".");
            Console.ReadLine();

            StreamWriter writer = new StreamWriter(fileSave);

            foreach (string save in testResults)
            {
                writer.WriteLine(save);    
            }

            writer.Close();

            reader.Close();

            
        }

        static void testNN()
        {
            double percentCorrect = 0.0;
            int amtLines = 1;
            int numCorrect = 0;
            StreamReader test = new StreamReader("optdigits_test.txt");
            do
            {
                string testLine = test.ReadLine();
                List<int> inputs = testLine.Split(',').Select(x => int.Parse(x)).ToList();
                int targetValue = inputs.Last();

                NN.forwardPropagate(inputs);

                int outputvalue = 0;
                int outputLayer_Index = NN.layers.Count - 1;
                Node best_Guess_Node = NN.layers[outputLayer_Index].layerNodeArray[0];
                Layer outputLayer = NN.layers[outputLayer_Index];


                for (int i = 0; i < outputLayer.nodeCount; i++)
                {
                    if (outputLayer.layerNodeArray[i].value > best_Guess_Node.value)
                    {
                        best_Guess_Node = outputLayer.layerNodeArray[i];
                        outputvalue = i;
                    }
                    NN.layers[outputLayer_Index].layerNodeArray[i].inputError = (NN.sigmoidDerivate_Func(i - targetValue)) * NN.learningRate;

                }
                if (outputvalue == targetValue)
                {
                    numCorrect++;
                }
                amtLines++;
            } while (!test.EndOfStream);
            percentCorrect = (double)numCorrect / (double)amtLines;
            Console.Out.WriteLine(percentCorrect + "");
            testResults.Add(percentCorrect + "");
        }
    }
}
