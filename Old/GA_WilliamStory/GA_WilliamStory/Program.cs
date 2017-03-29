using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_WilliamStory
{
    class Program
    {
        static List<string> candidateValues = new List<string>();
        static List<string> avgFitness_Per_Generation = new List<string>();
        const string CSV_Save = "fitness_values.csv";

        static void Main(string[] args)
        {

            avgFitness_Per_Generation.Add("Average Fitness per Generation");

            //Initialize inital population.
            Random rand = new Random();

            for(int i = 0; i < 4; i++)
            {
                int initValue = rand.Next(31);
                string initValue_AsString = Convert.ToString(initValue, 2);
                if (initValue_AsString.Length < 5)
                {
                    initValue_AsString =  initValue_AsString.PadLeft(5, '0');
                }
                candidateValues.Add(initValue_AsString);
            }

            do
            {
                for (int i = 0; i < 10; i++)
                {
                    //Find the most/least fit candidates here.
                    string candidateToRemove = candidateValues[0];
                    double lowestFitness = fitness(candidateValues[0]);

                    string candiateToDouble = candidateValues[1];
                    double highestFitness = fitness(candidateValues[1]);

                    foreach (string candidate in candidateValues)
                    {
                        
                        double candidateFitness = fitness(candidate);
                        if (candidateFitness < lowestFitness)
                        {
                            candidateToRemove = candidate;
                            lowestFitness = candidateFitness;
                        }
                        else
                        {
                            candiateToDouble = candidate;
                            highestFitness = candidateFitness;
                        }
                    }
                    //Console.Out.WriteLine("Generation highest fitness: {0}", highestFitness);
                    averageFitness();
                    candidateValues.Remove(candidateToRemove);
                    candidateValues.Remove(candiateToDouble);

                    //Reproduce the highest fit candidate with the other remaining candidates.
                    List<string> firstCross = Reproduce(candiateToDouble, candidateValues[0], 2);
                    List<string> secondCross = Reproduce(candidateValues[1], candiateToDouble, 3);

                    candidateValues.Clear();
                    foreach (string item in firstCross)
                    {
                        candidateValues.Add(item);
                    }
                    foreach (string item in secondCross)
                    {
                        candidateValues.Add(item);
                    }
                    mutateCandidates();
                }
                Console.Out.WriteLine("________________________________________");
            } while (!Console.ReadLine().Equals("."));
            StreamWriter writer = new StreamWriter(CSV_Save);

            foreach (string value in avgFitness_Per_Generation)
            {
                writer.WriteLine(value);
            }

            writer.Close();
        }

        static double fitness(string candidate)
        {
            int candidateAsString = Convert.ToInt32(candidate, 2);
            double funcTest = Math.Pow(candidateAsString, 2);
            return funcTest / Math.Pow(31, 2);
        }

        static List<string> Reproduce(string parentX, string parentY, int selection)
        {
            //make the selection
            string firstSelectionX = parentX.Substring(0, selection);
            string firstSelectionY = parentX.Substring(selection, parentX.Length - selection);


            string secondSelectionX = parentY.Substring(selection, parentY.Length - selection);
            string secondSelectionY = parentY.Substring(0, selection);

            //Crossover the selections
            List<string> crossedStrings = new List<string>();
            crossedStrings.Add(firstSelectionX + secondSelectionX);
            crossedStrings.Add(firstSelectionY + secondSelectionY);

            return crossedStrings;
        }

        static void mutateCandidates()
        {
            Random rand = new Random();
            for (int i = 0; i < candidateValues.Count; i++)
            {
                double mutate = rand.NextDouble() * (100.00 - 0.0) + 0.0;
                double mutateChance = rand.NextDouble() * (100.00 - 0.0) + 0.0;

                if (mutateChance >= mutate)
                {
                    int mutateLocation = rand.Next(5);
                    string mutateChild = candidateValues[i];
                    char mutatedValue = mutateChild[mutateLocation];
                    mutatedValue = (mutatedValue == '1') ? '0' : '1';

                    StringBuilder muttableString = new StringBuilder(mutateChild);
                    muttableString[mutateLocation] = mutatedValue;
                    candidateValues[i] = muttableString.ToString();
                }
            }
        }

        static void averageFitness()
        {
            double totalFit = 0.0;
            foreach (string item in candidateValues)
            {
                totalFit += fitness(item);
            }
            totalFit = totalFit / 4;
            avgFitness_Per_Generation.Add(totalFit + "");
            Console.Out.WriteLine("Average fitness for this generation {0}", totalFit);
        }
    }
}
